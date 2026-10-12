using Microsoft.Extensions.Options;

namespace MajesticRoofing.Seo;

public sealed class CanonicalHostMiddleware
{
    private readonly RequestDelegate _next;

    public CanonicalHostMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, IOptions<SeoOptions> options)
    {
        var path = context.Request.Path.Value ?? "/";
        var slashTrim = path.Length > 1 && path.EndsWith('/');
        var host = context.Request.Host.Host;
        var isLocal = host.Equals("localhost", StringComparison.OrdinalIgnoreCase) ||
                      host.Equals("127.0.0.1", StringComparison.OrdinalIgnoreCase);

        if (slashTrim)
        {
            var cleanPath = path.TrimEnd('/');
            if (string.IsNullOrEmpty(cleanPath)) cleanPath = "/";
            var authority = $"{context.Request.Scheme}://{context.Request.Host.Value}";
            var siteUrl = options.Value.SiteUrl;
            if (!isLocal &&
                !string.IsNullOrWhiteSpace(siteUrl) &&
                Uri.TryCreate(siteUrl.TrimEnd('/'), UriKind.Absolute, out var canonicalFromSlash))
            {
                authority = canonicalFromSlash.GetLeftPart(UriPartial.Authority);
            }

            context.Response.Redirect(
                authority + context.Request.PathBase.Value + cleanPath + context.Request.QueryString.Value,
                permanent: true);
            return;
        }

        var configured = options.Value.SiteUrl;
        if (isLocal || string.IsNullOrWhiteSpace(configured))
        {
            await _next(context);
            return;
        }

        if (!Uri.TryCreate(configured.TrimEnd('/'), UriKind.Absolute, out var canonical))
        {
            await _next(context);
            return;
        }

        if (!host.Equals(canonical.Host, StringComparison.OrdinalIgnoreCase))
        {
            var dest = canonical.GetLeftPart(UriPartial.Authority)
                + context.Request.PathBase.Value
                + path
                + context.Request.QueryString.Value;
            context.Response.Redirect(dest, permanent: true);
            return;
        }

        await _next(context);
    }
}
