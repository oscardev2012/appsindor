namespace IndorNews.Data;

public static class SitePostCatalog
{
    public static readonly (string Key, string Label)[] Sites =
    [
        ("safe", "SAFE Water Heater"),
        ("roofing", "Majestic Roofing"),
        ("realtor", "Manuela Ruiz Realtor"),
        ("carolina", "Carolinas Coverage"),
        ("realestatedesk", "Charlotte Real Estate Desk"),
        ("safecomfort", "Safe Comfort (HVAC)"),
    ];

    public static bool TryNormalize(string? site, out string key)
    {
        key = "";
        if (string.IsNullOrWhiteSpace(site))
        {
            return false;
        }

        foreach (var (k, _) in Sites)
        {
            if (k.Equals(site.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                key = k;
                return true;
            }
        }

        return false;
    }

    public static string Label(string key) =>
        Sites.FirstOrDefault(s => s.Key == key).Label ?? key;
}
