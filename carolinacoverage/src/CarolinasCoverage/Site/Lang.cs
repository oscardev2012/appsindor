namespace CarolinasCoverage.Site;

public static class Lang
{
    public static bool IsSpanish(HttpRequest request) =>
        request.Path.StartsWithSegments("/es");

    public static bool IsEnglish(HttpRequest request) => !IsSpanish(request);

    public static string Home(bool en) => en ? "/" : "/es";

    public static string Coverage(bool en, string product) =>
        SeoHubCatalog.TryProduct(product, out var hub)
            ? hub.Path(en)
            : en ? $"/coverage/{product}" : $"/es/coverage/{product}";

    public static string Service(bool en, string type) =>
        en ? $"/service/{type}" : $"/es/service/{type}";

    public static string Portal(bool en) => en ? "/portal-demo" : "/es/portal-demo";

    public static string Quote(bool en) => en ? "/#quote" : "/es#quote";

    public static string Contact(bool en) => en ? "/contact" : "/es/contacto";

    public static string Faqs(bool en) => FaqHubCatalog.Path(en);

    public static string ToEnglish(HttpRequest request) =>
        Pair(request).En + (request.QueryString.Value ?? "");

    public static string ToSpanish(HttpRequest request) =>
        Pair(request).Es + (request.QueryString.Value ?? "");

    public static (string Es, string En) Pair(HttpRequest request)
    {
        var path = Normalize(request.Path.Value);
        if (SeoHubCatalog.TryGetByPath(path, out var hub, out _))
        {
            return (hub.SlugEs, hub.SlugEn);
        }

        if (path.Equals("/es", StringComparison.OrdinalIgnoreCase) || path == "/")
        {
            return ("/es", "/");
        }

        if (path.Equals(FaqHubCatalog.PathEn, StringComparison.OrdinalIgnoreCase)
            || path.Equals(FaqHubCatalog.PathEs, StringComparison.OrdinalIgnoreCase))
        {
            return (FaqHubCatalog.PathEs, FaqHubCatalog.PathEn);
        }

        if (path.StartsWith("/es/", StringComparison.OrdinalIgnoreCase))
        {
            var rest = path[3..];
            return (path, string.IsNullOrEmpty(rest) ? "/" : rest);
        }

        return ("/es" + path, path);
    }

    public static string Normalize(string? path)
    {
        path ??= "/";
        if (path.Length > 1)
        {
            path = path.TrimEnd('/');
        }

        return string.IsNullOrEmpty(path) ? "/" : path;
    }
}
