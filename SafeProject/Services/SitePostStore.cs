using System.Text.Json;
using CalentonesAgua.Data;
using CalentonesAgua.Models;
using Microsoft.Data.SqlClient;

namespace CalentonesAgua.Services;

public sealed class SitePostStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };

    private readonly string? _connectionString;
    private readonly string _path;
    private readonly object _gate = new();

    public SitePostStore(IWebHostEnvironment env, IConfiguration config)
    {
        _connectionString = config.GetConnectionString("IndorDB")?.Trim();
        var configured = config["Updates:StorePath"];
        _path = string.IsNullOrWhiteSpace(configured)
            ? Path.Combine(env.ContentRootPath, "App_Data", "site-posts.json")
            : configured;
    }

    public IReadOnlyList<SitePost> All()
    {
        if (UseSql)
        {
            return QuerySql("""
                SELECT Id, SiteKey, Title, Body, ImageUrl, CreatedUtc
                FROM dbo.SitePosts
                ORDER BY CreatedUtc DESC
                """);
        }

        lock (_gate)
        {
            return LoadUnlocked()
                .OrderByDescending(p => p.CreatedUtc)
                .ToList();
        }
    }

    public SitePost? Latest(string site)
    {
        if (!SitePostCatalog.TryNormalize(site, out var key))
        {
            return null;
        }

        if (UseSql)
        {
            try
            {
                return QuerySql("""
                    SELECT TOP (1) Id, SiteKey, Title, Body, ImageUrl, CreatedUtc
                    FROM dbo.SitePosts
                    WHERE SiteKey = @site
                    ORDER BY CreatedUtc DESC
                    """, key).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        lock (_gate)
        {
            return LoadUnlocked()
                .Where(p => p.Site == key)
                .OrderByDescending(p => p.CreatedUtc)
                .FirstOrDefault();
        }
    }

    public SitePost Add(string site, string title, string body, string? imageUrl)
    {
        if (!SitePostCatalog.TryNormalize(site, out var key))
        {
            throw new ArgumentException("Unknown site.", nameof(site));
        }

        title = (title ?? "").Trim();
        body = (body ?? "").Trim();
        if (title.Length == 0)
        {
            throw new ArgumentException("Title is required.", nameof(title));
        }

        if (title.Length > 180)
        {
            title = title[..180];
        }

        if (body.Length > 4000)
        {
            body = body[..4000];
        }

        var post = new SitePost
        {
            Id = Guid.NewGuid().ToString("n"),
            Site = key,
            Title = title,
            Body = body,
            ImageUrl = SanitizeImageUrl(imageUrl),
            CreatedUtc = DateTimeOffset.UtcNow,
        };

        if (UseSql)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var cmd = new SqlCommand("""
                INSERT INTO dbo.SitePosts (Id, SiteKey, Title, Body, ImageUrl, CreatedUtc)
                VALUES (@id, @site, @title, @body, @image, @created)
                """, connection);
            cmd.Parameters.AddWithValue("@id", Guid.Parse(post.Id));
            cmd.Parameters.AddWithValue("@site", post.Site);
            cmd.Parameters.AddWithValue("@title", post.Title);
            cmd.Parameters.AddWithValue("@body", post.Body);
            cmd.Parameters.AddWithValue("@image", (object?)post.ImageUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@created", post.CreatedUtc);
            cmd.ExecuteNonQuery();
            return post;
        }

        lock (_gate)
        {
            var posts = LoadUnlocked();
            posts.Add(post);
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            File.WriteAllText(_path, JsonSerializer.Serialize(new FileShape { Posts = posts }, JsonOptions));
        }

        return post;
    }

    internal static string? SanitizeImageUrl(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return null;
        }

        if (!Uri.TryCreate(imageUrl.Trim(), UriKind.Absolute, out var uri))
        {
            return null;
        }

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
        {
            return null;
        }

        return uri.ToString();
    }

    private bool UseSql => !string.IsNullOrWhiteSpace(_connectionString);

    private List<SitePost> QuerySql(string sql, string? site = null)
    {
        var posts = new List<SitePost>();
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using var cmd = new SqlCommand(sql, connection);
        if (site is not null)
        {
            cmd.Parameters.AddWithValue("@site", site);
        }

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            posts.Add(new SitePost
            {
                Id = reader.GetGuid(0).ToString("n"),
                Site = reader.GetString(1),
                Title = reader.GetString(2),
                Body = reader.IsDBNull(3) ? "" : reader.GetString(3),
                ImageUrl = reader.IsDBNull(4) ? null : reader.GetString(4),
                CreatedUtc = reader.GetFieldValue<DateTimeOffset>(5),
            });
        }

        return posts;
    }

    private List<SitePost> LoadUnlocked()
    {
        if (!File.Exists(_path))
        {
            return [];
        }

        try
        {
            var json = File.ReadAllText(_path);
            var file = JsonSerializer.Deserialize<FileShape>(json, JsonOptions);
            return file?.Posts ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }

    private sealed class FileShape
    {
        public List<SitePost> Posts { get; set; } = [];
    }
}
