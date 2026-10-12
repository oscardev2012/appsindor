namespace RealtorNews.Data;

public static class PostCatalog
{
    public static readonly string[] Categories =
    [
        "Buying",
        "Selling",
        "Relocation",
        "Homeownership",
        "Investing",
        "Market",
        "Contracts",
        "Home Services",
        "For Realtors"
    ];

    public static readonly string[] Cities =
    [
        "Charlotte",
        "Gastonia",
        "Concord",
        "Huntersville",
        "Matthews",
        "Fort Mill",
        "Rock Hill",
        "Waxhaw",
        "Lake Norman"
    ];

    public static string NormalizeCategory(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "Buying";
        }

        var match = Categories.FirstOrDefault(c => c.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase));
        return match ?? "Buying";
    }

    public static string NormalizeType(string? value)
        => string.Equals(value, "article", StringComparison.OrdinalIgnoreCase) ? "article" : "feed";
}
