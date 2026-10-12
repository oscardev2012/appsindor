using ManuelaRuiz.Web.Services;

namespace ManuelaRuiz.Web.Middleware;

public sealed class HostCanonicalMiddleware(RequestDelegate next, IWebHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        if (!env.IsDevelopment() && ShouldRedirect(ctx.Request.Host.Host))
        {
            var path = ctx.Request.Path.Value ?? "/";
            var qs = ctx.Request.QueryString.HasValue ? ctx.Request.QueryString.Value : "";
            ctx.Response.Redirect(SiteLocale.PublicOrigin + path + qs, permanent: true);
            return;
        }

        await next(ctx);
    }

    private static bool ShouldRedirect(string host) =>
        host.Equals("www." + SiteLocale.PublicHost, StringComparison.OrdinalIgnoreCase)
        || host.EndsWith(".azurewebsites.net", StringComparison.OrdinalIgnoreCase);
}
