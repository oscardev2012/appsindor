namespace IndorNews.Models;

public sealed class SitePost
{
    public string Id { get; set; } = "";
    public string Site { get; set; } = "";
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";
    public string? ImageUrl { get; set; }
    public DateTimeOffset CreatedUtc { get; set; }
}
