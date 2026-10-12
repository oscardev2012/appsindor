namespace CarolinasCoverage.Site;

public static class HostCanonical
{
    public const string ApexHost = "carolinascoverageinsurance.com";

    public static bool TryRedirect(HttpRequest request, out string destination)
    {
        destination = "";
        var host = request.Host.Host;
        if (string.IsNullOrEmpty(host))
        {
            return false;
        }

        var path = request.Path.Value ?? "/";
        var query = request.QueryString.Value ?? "";
        var changed = false;
        var scheme = request.Scheme;

        if (host.Equals("www." + ApexHost, StringComparison.OrdinalIgnoreCase))
        {
            host = ApexHost;
            scheme = "https";
            changed = true;
        }
        else if (host.Equals(ApexHost, StringComparison.OrdinalIgnoreCase) &&
                 !string.Equals(scheme, "https", StringComparison.OrdinalIgnoreCase))
        {
            scheme = "https";
            changed = true;
        }

        if (path.Length > 1 && path.EndsWith('/'))
        {
            path = path.TrimEnd('/');
            changed = true;
        }

        if (!changed)
        {
            return false;
        }

        destination = $"{scheme}://{host.ToLowerInvariant()}{path}{query}";
        return true;
    }
}
