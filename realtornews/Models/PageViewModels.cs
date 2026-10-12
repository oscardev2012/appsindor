using RealtorNews.Models;

namespace RealtorNews.Models;

public sealed class HomePageViewModel
{
    public string? DbError { get; set; }
    public IReadOnlyList<NewsPost> Latest { get; set; } = [];
    public IReadOnlyList<NewsPost> Articles { get; set; } = [];
    public IReadOnlyList<RealtorAccount> Realtors { get; set; } = [];
    public int RealtorCount { get; set; }
    public int PostCount { get; set; }
}

public sealed class FeedPageViewModel
{
    public string? DbError { get; set; }
    public string Category { get; set; } = "all";
    public string Scope { get; set; } = "all";
    public IReadOnlyList<NewsPost> Posts { get; set; } = [];
    public IReadOnlyList<RealtorAccount> Experts { get; set; } = [];
}

public sealed class MagazinePageViewModel
{
    public string? DbError { get; set; }
    public NewsPost? Lead { get; set; }
    public IReadOnlyList<NewsPost> Stories { get; set; } = [];
}

public sealed class PostDetailViewModel
{
    public NewsPost Post { get; set; } = new();
    public IReadOnlyList<PostComment> Comments { get; set; } = [];
    public IReadOnlyList<NewsPost> More { get; set; } = [];
}

public sealed class RealtorDirectoryViewModel
{
    public string? DbError { get; set; }
    public IReadOnlyList<RealtorAccount> Realtors { get; set; } = [];
}

public sealed class RealtorProfileViewModel
{
    public RealtorAccount Realtor { get; set; } = new();
    public IReadOnlyList<NewsPost> Posts { get; set; } = [];
}

public sealed class RegisterViewModel
{
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Brokerage { get; set; } = "";
    public string LicenseNumber { get; set; } = "";
    public string City { get; set; } = "Charlotte";
    public string State { get; set; } = "NC";
    public string Phone { get; set; } = "";
    public string Languages { get; set; } = "English";
    public string Headline { get; set; } = "";
    public string Bio { get; set; } = "";
}

public sealed class LoginViewModel
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string? ReturnUrl { get; set; }
}

public sealed class ComposeViewModel
{
    public Guid? Id { get; set; }
    public string PostType { get; set; } = "article";
    public string Category { get; set; } = "Buying";
    public string Title { get; set; } = "";
    public string Summary { get; set; } = "";
    public string Body { get; set; } = "";
    public string CoverImageUrl { get; set; } = "";
    public string City { get; set; } = "Charlotte";
    public bool Publish { get; set; } = true;
}

public sealed class StudioHomeViewModel
{
    public RealtorAccount Realtor { get; set; } = new();
    public IReadOnlyList<NewsPost> Posts { get; set; } = [];
    public IReadOnlyList<PublicQuestion> Questions { get; set; } = [];
}

public sealed class AskViewModel
{
    public string FirstName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Topic { get; set; } = "Buying";
    public string Body { get; set; } = "";
}

public sealed class VendorRequestViewModel
{
    public string Service { get; set; } = "Inspection & Risk Review";
    public string FirstName { get; set; } = "";
    public string Zip { get; set; } = "";
    public string Contact { get; set; } = "";
    public string RealtorCode { get; set; } = "";
    public string Description { get; set; } = "";
}
