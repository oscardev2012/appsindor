using IndorNews.Data;
using IndorNews.Models;
using Microsoft.Data.SqlClient;

namespace IndorNews.Services;

public sealed class SitePostStore
{
    private readonly string _connectionString;

    public SitePostStore(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("IndorDB")?.Trim() ?? "";
    }

    private void EnsureSql()
    {
        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            throw new InvalidOperationException("Falta ConnectionStrings:IndorDB.");
        }
    }

    public IReadOnlyList<SitePost> All()
    {
        EnsureSql();
        return Query("""
            SELECT Id, SiteKey, Title, Body, ImageUrl, CreatedUtc
            FROM dbo.SitePosts
            ORDER BY CreatedUtc DESC
            """);
    }

    public SitePost? Latest(string site)
    {
        EnsureSql();
        if (!SitePostCatalog.TryNormalize(site, out var key))
        {
            return null;
        }

        return Query("""
            SELECT TOP (1) Id, SiteKey, Title, Body, ImageUrl, CreatedUtc
            FROM dbo.SitePosts
            WHERE SiteKey = @site
            ORDER BY CreatedUtc DESC
            """, key).FirstOrDefault();
    }

    public SitePost Add(string site, string title, string body, string? imageUrl)
    {
        EnsureSql();
        if (!SitePostCatalog.TryNormalize(site, out var key))
        {
            throw new ArgumentException("Unknown site.", nameof(site));
        }

        title = (title ?? "").Trim();
        body = (body ?? "").Trim();
        var copyError = PostCopyLimits.Validate(title, body);
        if (copyError is not null)
        {
            throw new ArgumentException(copyError);
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

    private List<SitePost> Query(string sql, string? site = null)
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
}
