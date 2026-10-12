namespace ManuelaRuiz.Web.Services;

public static class Questionnaire
{
    public const string BuyerEs = "https://forms.gle/VWEBrw3q5auVN6rd6";
    public const string BuyerEn = "https://forms.gle/ecdzZr2wgnk6UNnUA";
    public const string SellerEs = "https://forms.gle/9ga8kKJFYQPHF6hKA";
    public const string SellerEn = "https://forms.gle/M5qvjP7LhXt8DeQM6";

    public const string BuyerEsEmbed = "https://docs.google.com/forms/d/e/1FAIpQLSddFQ_aMCiXYCFjP95IzT67S1LtOGGUxbWfMNpoZJ_qGQ75yg/viewform?embedded=true";
    public const string BuyerEnEmbed = "https://docs.google.com/forms/d/e/1FAIpQLSclvnnTT_fY3W6SuhpSQoYBIc4k9glhjHwtMQON-xlcB_ylsA/viewform?embedded=true";
    public const string SellerEsEmbed = "https://docs.google.com/forms/d/e/1FAIpQLSey2PeZ_TAd8V4WESYTLwS_rGF-WjBx6Yv6xl9o2ytBBBLrpg/viewform?embedded=true";
    public const string SellerEnEmbed = "https://docs.google.com/forms/d/e/1FAIpQLSetP097XNIixgBji1H1kVhNvOy4iCKY10N6W7FKDJKDA30UVw/viewform?embedded=true";

    public static bool IsSeller(string? kind)
    {
        if (string.IsNullOrWhiteSpace(kind)) return false;
        kind = kind.Trim().ToLowerInvariant();
        return kind is "sell" or "seller" or "sellers" or "valuation" or "home-valuation";
    }

    public static string Url(bool seller, bool english) =>
        seller
            ? (english ? SellerEn : SellerEs)
            : (english ? BuyerEn : BuyerEs);

    public static string Embed(bool seller, bool english) =>
        seller
            ? (english ? SellerEnEmbed : SellerEsEmbed)
            : (english ? BuyerEnEmbed : BuyerEsEmbed);
}
