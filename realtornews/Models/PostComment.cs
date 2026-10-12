namespace RealtorNews.Models;

public sealed class PostComment
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid RealtorId { get; set; }
    public string Body { get; set; } = "";
    public DateTimeOffset CreatedUtc { get; set; }
    public string RealtorName { get; set; } = "";
    public string RealtorSlug { get; set; } = "";

    public string Initials
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
}
