using System.Globalization;
using System.Security.Claims;

namespace RealtorNews.Services;

public static class RealtorPrincipal
{
    public const string Scheme = "RealtorNews";

    public static Guid? RealtorId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var id) ? id : null;
    }

    public static string? RealtorSlug(this ClaimsPrincipal user)
        => user.FindFirstValue("slug");
}

public static class EasternTime
{
    private static readonly TimeZoneInfo Zone = TimeZoneInfo.FindSystemTimeZoneById(
        OperatingSystem.IsWindows() ? "Eastern Standard Time" : "America/New_York");

    public static DateTimeOffset ToEastern(DateTimeOffset utc)
        => TimeZoneInfo.ConvertTime(utc, Zone);

    public static string LongDate(DateTimeOffset utc)
        => ToEastern(utc).ToString("MMMM d, yyyy", CultureInfo.GetCultureInfo("en-US"));

    public static string Stamp(DateTimeOffset utc)
        => ToEastern(utc).ToString("MMMM d, yyyy · h:mm tt", CultureInfo.GetCultureInfo("en-US"));
}
