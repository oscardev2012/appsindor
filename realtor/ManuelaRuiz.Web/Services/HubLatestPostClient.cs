using Microsoft.Data.SqlClient;

namespace ManuelaRuiz.Web.Services;

public sealed class HubLatestPost
{
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";
    public string? ImageUrl { get; set; }
    public DateTimeOffset PublishedUtc { get; set; }
}

public sealed class HubLatestPostClient
{
    private readonly string? _connectionString;
    private readonly string _site;

    public HubLatestPostClient(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("IndorDB")?.Trim();
        _site = config["Updates:SiteKey"] ?? "realtor";
    }

    public HubLatestPost? Get()
    {
        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            return null;
        }

        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var cmd = new SqlCommand("""
                SELECT TOP (1) Title, Body, ImageUrl, CreatedUtc
                FROM dbo.SitePosts
                WHERE SiteKey = @site
                ORDER BY CreatedUtc DESC
                """, connection);
            cmd.Parameters.AddWithValue("@site", _site);
            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            var title = reader.GetString(0);
            if (string.IsNullOrWhiteSpace(title))
            {
                return null;
            }

            return new HubLatestPost
            {
                Title = title,
                Body = reader.IsDBNull(1) ? "" : reader.GetString(1),
                ImageUrl = reader.IsDBNull(2) ? null : reader.GetString(2),
                PublishedUtc = reader.GetFieldValue<DateTimeOffset>(3),
            };
        }
        catch
        {
            return null;
        }
    }
}
