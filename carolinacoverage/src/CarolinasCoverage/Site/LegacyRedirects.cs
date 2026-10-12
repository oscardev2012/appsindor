namespace CarolinasCoverage.Site;

public static class LegacyRedirects
{
    public static bool TryMatch(PathString path, out string destination)
    {
        var value = Lang.Normalize(path.Value);
        destination = "";

        if (value.Equals("/en", StringComparison.OrdinalIgnoreCase))
        {
            destination = "/";
            return true;
        }

        if (value.StartsWith("/en/", StringComparison.OrdinalIgnoreCase))
        {
            destination = MapEnglishRest(value[3..]);
            return true;
        }

        if (value.StartsWith("/es/coverage/", StringComparison.OrdinalIgnoreCase))
        {
            var id = value["/es/coverage/".Length..];
            if (SeoHubCatalog.TryProduct(id, out var hub))
            {
                destination = hub.SlugEs;
                return true;
            }

            return false;
        }

        if (value.StartsWith("/coverage/", StringComparison.OrdinalIgnoreCase))
        {
            var id = value["/coverage/".Length..];
            if (SeoHubCatalog.TryProduct(id, out var hub))
            {
                destination = hub.SlugEn;
                return true;
            }

            return false;
        }

        return false;
    }

    private static string MapEnglishRest(string rest)
    {
        if (rest.StartsWith("/coverage/", StringComparison.OrdinalIgnoreCase))
        {
            var id = rest["/coverage/".Length..];
            return SeoHubCatalog.TryProduct(id, out var hub) ? hub.SlugEn : rest;
        }

        return rest;
    }
}
