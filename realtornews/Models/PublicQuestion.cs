namespace RealtorNews.Models;

public sealed class PublicQuestion
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Topic { get; set; }
    public string Body { get; set; } = "";
    public DateTimeOffset CreatedUtc { get; set; }
    public string Status { get; set; } = "open";
    public Guid? AnswerPostId { get; set; }
}
