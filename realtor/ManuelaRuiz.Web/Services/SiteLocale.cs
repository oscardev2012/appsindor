namespace ManuelaRuiz.Web.Services;

public static class SiteLocale
{
    public const string EnglishPrefix = "/en";
    public const string PublicHost = "manuelaruizrealty.com";
    public const string PublicOrigin = "https://manuelaruizrealty.com";

    public static bool IsEnglish(HttpContext ctx) =>
        string.Equals(ctx.Items["Locale"] as string, "en", StringComparison.OrdinalIgnoreCase);

    public static string PublicPath(HttpContext ctx)
    {
        if (ctx.Items["PublicPath"] is string path && path.Length > 0)
            return path;

        var raw = ctx.Request.Path.Value ?? "/";
        return string.IsNullOrWhiteSpace(raw) ? "/" : raw;
    }

    public static string CatalogKey(HttpContext ctx) => StripEnglishPrefix(PublicPath(ctx));

    public static string StripEnglishPrefix(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || path == "/") return "/";
        if (path.Equals(EnglishPrefix, StringComparison.OrdinalIgnoreCase)) return "/";
        if (path.StartsWith(EnglishPrefix + "/", StringComparison.OrdinalIgnoreCase))
        {
            var rest = path[EnglishPrefix.Length..];
            return string.IsNullOrEmpty(rest) ? "/" : rest;
        }
        return path;
    }

    public static string WithEnglishPrefix(string path)
    {
        path = Normalize(path);
        return path == "/" ? EnglishPrefix : EnglishPrefix + path;
    }

    public static string Localize(HttpContext ctx, string path) =>
        IsEnglish(ctx) ? WithEnglishPrefix(path) : Normalize(path);

    public static string AlternatePath(HttpContext ctx)
    {
        var key = CatalogKey(ctx);
        return IsEnglish(ctx) ? key : WithEnglishPrefix(key);
    }

    public static string Origin(HttpContext ctx)
    {
        var host = ctx.Request.Host.Host;
        if (host.Equals(PublicHost, StringComparison.OrdinalIgnoreCase)
            || host.Equals("www." + PublicHost, StringComparison.OrdinalIgnoreCase)
            || host.EndsWith(".azurewebsites.net", StringComparison.OrdinalIgnoreCase))
        {
            return PublicOrigin;
        }

        var scheme = ctx.Request.Scheme;
        if (string.Equals(ctx.Request.Headers["X-Forwarded-Proto"], "https", StringComparison.OrdinalIgnoreCase))
            scheme = "https";
        return $"{scheme}://{ctx.Request.Host.Value.TrimEnd('/')}";
    }

    public static string Absolute(HttpContext ctx, string path)
    {
        path = string.IsNullOrWhiteSpace(path) ? "/" : path;
        if (!path.StartsWith('/')) path = "/" + path;
        return Origin(ctx) + path;
    }

    public static string Normalize(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return "/";
        var hash = "";
        var hashAt = path.IndexOf('#');
        if (hashAt >= 0)
        {
            hash = path[hashAt..];
            path = path[..hashAt];
        }

        if (!path.StartsWith('/')) path = "/" + path;
        if (path.Length > 1) path = path.TrimEnd('/');
        return path + hash;
    }
}
