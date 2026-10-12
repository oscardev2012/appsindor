using RealtorNews.Data;

namespace RealtorNews.Models;

public sealed class NewsPost
{
    public Guid Id { get; set; }
    public Guid RealtorId { get; set; }
    public string PostType { get; set; } = "feed";
    public string Category { get; set; } = "Buying";
    public string Title { get; set; } = "";
    public string Slug { get; set; } = "";
    public string? Summary { get; set; }
    public string Body { get; set; } = "";
    public string? CoverImageUrl { get; set; }
    public string? City { get; set; }
    public bool IsPublished { get; set; }
    public DateTimeOffset? PublishedUtc { get; set; }
    public DateTimeOffset CreatedUtc { get; set; }
    public DateTimeOffset UpdatedUtc { get; set; }

    public string RealtorName { get; set; } = "";
    public string RealtorSlug { get; set; } = "";
    public string? RealtorBrokerage { get; set; }
    public string? RealtorPhotoUrl { get; set; }
    public string? RealtorHeadline { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public bool LikedByMe { get; set; }
    public bool FollowingAuthor { get; set; }

    public bool IsArticle => string.Equals(PostType, "article", StringComparison.OrdinalIgnoreCase);

    public string RealtorInitials
    {
        get
        {
            var parts = RealtorName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
            {
                return "R";
            }

            if (parts.Length == 1)
            {
                return parts[0][..Math.Min(2, parts[0].Length)].ToUpperInvariant();
            }

            return $"{char.ToUpperInvariant(parts[0][0])}{char.ToUpperInvariant(parts[^1][0])}";
        }
    }

    public string Teaser
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(Summary))
            {
                return Summary.Trim();
            }

            var text = NewsFormatter.ToPlain(Body);
            return text.Length <= 220 ? text : text[..217].TrimEnd() + "…";
        }
    }
}
