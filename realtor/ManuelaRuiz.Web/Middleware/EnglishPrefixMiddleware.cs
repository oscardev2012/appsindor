using ManuelaRuiz.Web.Services;

namespace ManuelaRuiz.Web.Middleware;

public sealed class EnglishPrefixMiddleware(RequestDelegate next)
{
    private static readonly PathString[] PassThrough =
    [
        new("/css"), new("/js"), new("/img"), new("/lib"),
        new("/sitemap.xml"), new("/robots.txt"), new("/llms.txt")
    ];

    public async Task InvokeAsync(HttpContext ctx)
    {
        var path = ctx.Request.Path;
        if (PassThrough.Any(p => path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase)))
        {
            await next(ctx);
            return;
        }

        if (path.StartsWithSegments(SiteLocale.EnglishPrefix, StringComparison.OrdinalIgnoreCase, out var rest))
        {
            ctx.Items["Locale"] = "en";
            ctx.Items["PublicPath"] = path.Value ?? SiteLocale.EnglishPrefix;
            ctx.Request.Path = rest.HasValue && rest.Value!.Length > 0 ? rest : "/";
        }
        else
        {
            ctx.Items["Locale"] = "es";
            ctx.Items["PublicPath"] = path.HasValue && path.Value!.Length > 0 ? path.Value : "/";
        }

        await next(ctx);
    }
}
