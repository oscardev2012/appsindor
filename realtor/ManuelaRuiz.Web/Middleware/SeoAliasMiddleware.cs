using ManuelaRuiz.Web.Services;

namespace ManuelaRuiz.Web.Middleware;

public sealed class SeoAliasMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        if (HttpMethods.IsGet(ctx.Request.Method) || HttpMethods.IsHead(ctx.Request.Method))
        {
            var path = ctx.Request.Path.Value ?? "/";
            if (path.Length > 1) path = path.TrimEnd('/');
            var qs = ctx.Request.QueryString.HasValue ? ctx.Request.QueryString.Value : "";

            if (path.Equals("/es", StringComparison.OrdinalIgnoreCase))
            {
                ctx.Response.Redirect("/" + qs, permanent: true);
                return;
            }

            if (path.StartsWith("/es/", StringComparison.OrdinalIgnoreCase))
            {
                var dest = SeoGuide.ResolveSpanishSlug(path[4..]);
                ctx.Response.Redirect(dest + qs, permanent: true);
                return;
            }

            var alias = SeoGuide.ResolveAlias(path);
            if (alias is not null)
            {
                ctx.Response.Redirect(SiteLocale.Localize(ctx, alias) + qs, permanent: true);
                return;
            }

            var publicPath = SiteLocale.PublicPath(ctx);
            if (publicPath.Length > 1 && publicPath.EndsWith('/'))
            {
                ctx.Response.Redirect(publicPath.TrimEnd('/') + qs, permanent: true);
                return;
            }
        }

        await next(ctx);
    }
}
