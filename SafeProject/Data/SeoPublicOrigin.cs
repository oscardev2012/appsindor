using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CalentonesAgua.Data;

public static class SeoPublicOrigin
{
    public static string Resolve(IConfiguration config, HttpRequest request)
    {
        var configured = config["Seo:PublicOrigin"];
        if (!string.IsNullOrWhiteSpace(configured))
        {
            return configured.TrimEnd('/');
        }

        return $"{request.Scheme}://{request.Host}".TrimEnd('/');
    }

    public static bool ShouldRedirectToCanonicalHost(IConfiguration config, HttpRequest request, out string location)
    {
        location = "";
        var configured = config["Seo:PublicOrigin"];
        if (string.IsNullOrWhiteSpace(configured)
            || !Uri.TryCreate(configured.Trim().TrimEnd('/'), UriKind.Absolute, out var pub))
        {
            return false;
        }

        var host = request.Host.Host;
        if (string.IsNullOrEmpty(host)
            || host.Equals("localhost", StringComparison.OrdinalIgnoreCase)
            || host.Equals("127.0.0.1", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (System.Net.IPAddress.TryParse(host, out _))
        {
            return false;
        }

        if (host.Equals(pub.Host, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        location = pub.GetLeftPart(UriPartial.Authority)
            + request.PathBase
            + request.Path
            + request.QueryString;
        return true;
    }
}
