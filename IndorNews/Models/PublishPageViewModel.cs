using IndorNews.Models;

namespace IndorNews.Models;

public sealed class PublishPageViewModel
{
    public IReadOnlyList<SitePost> Posts { get; set; } = [];
    public string? Site { get; set; }
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";
    public string? ImageUrl { get; set; }
}
