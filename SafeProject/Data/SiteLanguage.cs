using Microsoft.AspNetCore.Http;

namespace CalentonesAgua.Data;

internal static class SiteLanguage
{
    public const string CookieName = "safe_lang";
    public const string English = "en";
    public const string Spanish = "es";

    public static bool IsSpanish(HttpContext http) =>
        string.Equals(Resolve(http), Spanish, StringComparison.OrdinalIgnoreCase);

    public static string Resolve(HttpContext http)
    {
        if (http.Items.TryGetValue(CookieName, out var cached) && cached is string ready)
            return ready;

        var path = http.Request.Path.Value ?? "/";
        if (path.StartsWith("/es/", StringComparison.OrdinalIgnoreCase) ||
            path.Equals("/es", StringComparison.OrdinalIgnoreCase))
        {
            return Remember(http, Spanish);
        }

        if (http.Request.Cookies.TryGetValue(CookieName, out var cookie) && IsCode(cookie))
            return Remember(http, cookie);

        return Remember(http, English);
    }

    public static void SetCookie(HttpResponse response, string lang)
    {
        lang = Normalize(lang);
        response.Cookies.Append(CookieName, lang, new CookieOptions
        {
            Path = "/",
            MaxAge = TimeSpan.FromDays(365),
            IsEssential = true,
            SameSite = SameSiteMode.Lax,
            HttpOnly = false
        });
    }

    public static string Normalize(string? code) =>
        IsCode(code) && string.Equals(code, Spanish, StringComparison.OrdinalIgnoreCase) ? Spanish : English;

    public static string SafeReturnUrl(string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl)) return "/";
        if (!returnUrl.StartsWith('/') || returnUrl.StartsWith("//", StringComparison.Ordinal)) return "/";
        if (returnUrl.Contains('\\') || returnUrl.Contains("://", StringComparison.Ordinal)) return "/";
        return returnUrl;
    }

    public static string RedirectAfterChoice(string lang, string returnUrl, SeoCatalog catalog)
    {
        lang = Normalize(lang);
        var path = SafeReturnUrl(returnUrl);
        var slug = SeoCatalog.Normalize(path.Split('?')[0]);

        if (lang == Spanish)
        {
            if (IsSameUrlUiPage(path)) return path;
            if (catalog.TryGet(slug, out var page) &&
                !string.IsNullOrEmpty(page.AlternateLangSlug) &&
                page.AlternateLangSlug.StartsWith("/es", StringComparison.OrdinalIgnoreCase))
            {
                return page.AlternateLangSlug;
            }

            if (slug.StartsWith("/es/", StringComparison.OrdinalIgnoreCase)) return path;
            return "/es/calentadores-de-agua-charlotte-nc/";
        }

        if (slug.StartsWith("/es/", StringComparison.OrdinalIgnoreCase) &&
            catalog.TryGet(slug, out var esPage) &&
            !string.IsNullOrEmpty(esPage.AlternateLangSlug))
        {
            return esPage.AlternateLangSlug;
        }

        if (slug.StartsWith("/es/", StringComparison.OrdinalIgnoreCase)) return "/";
        return path;
    }

    private static bool IsSameUrlUiPage(string path)
    {
        var p = path.Split('?')[0].TrimEnd('/');
        if (p.Length == 0) return true;
        return p is "/reviews" or "/about" or "/faqs" or "/water-heater-problems"
            or "/warranty" or "/warranty/register" or "/financing" or "/financing/safe-plan"
            or "/shelby" or "/china-grove" or "/site-map";
    }

    private static bool IsCode(string? code) =>
        string.Equals(code, English, StringComparison.OrdinalIgnoreCase) ||
        string.Equals(code, Spanish, StringComparison.OrdinalIgnoreCase);

    private static string Remember(HttpContext http, string lang)
    {
        lang = Normalize(lang);
        http.Items[CookieName] = lang;
        return lang;
    }
}
