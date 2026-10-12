using System.Security.Cryptography;
using System.Text;

namespace IndorNews.Services;

public static class SitePostAdminAuth
{
    public const string CookieName = "indor_news_admin";

    public static string? Token(IConfiguration config)
    {
        var password = config["Updates:AdminPassword"];
        if (string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        var key = Encoding.UTF8.GetBytes(password);
        var data = Encoding.UTF8.GetBytes("indor-news-admin-v1");
        return Convert.ToHexString(HMACSHA256.HashData(key, data));
    }

    public static bool IsSignedIn(HttpRequest request, IConfiguration config)
    {
        var expected = Token(config);
        if (expected is null)
        {
            return false;
        }

        if (!request.Cookies.TryGetValue(CookieName, out var actual) || string.IsNullOrEmpty(actual))
        {
            return false;
        }

        var left = Encoding.UTF8.GetBytes(actual);
        var right = Encoding.UTF8.GetBytes(expected);
        return left.Length == right.Length && CryptographicOperations.FixedTimeEquals(left, right);
    }
}
