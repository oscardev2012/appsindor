namespace RealtorNews.Models;

public sealed class RealtorAccount
{
    public Guid Id { get; set; }
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Slug { get; set; } = "";
    public string? Headline { get; set; }
    public string? Bio { get; set; }
    public string? Phone { get; set; }
    public string? Brokerage { get; set; }
    public string? LicenseNumber { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PhotoUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Languages { get; set; }
    public string? Specialties { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedUtc { get; set; }
    public DateTimeOffset? LastLoginUtc { get; set; }
    public int PostCount { get; set; }
    public int FollowerCount { get; set; }
    public int FollowingCount { get; set; }
    public bool FollowedByMe { get; set; }

    public string Initials
    {
        get
        {
            var parts = FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
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

    public string LocationLabel
    {
        get
        {
            var city = string.IsNullOrWhiteSpace(City) ? "Charlotte" : City.Trim();
            var state = string.IsNullOrWhiteSpace(State) ? "NC" : State.Trim();
            return $"{city}, {state}";
        }
    }
}
