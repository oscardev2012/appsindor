namespace CarolinasCoverage.Site;

public sealed class SiteSettings
{
    public const string SectionName = "Site";

    public string PublicBaseUrl { get; set; } = "";
    public string Name { get; set; } = "Carolinas Coverage Insurance Agency";
    public string Phone { get; set; } = "+1-704-595-3429";
    public string PhoneDisplay { get; set; } = "704-595-3429";
    public string Street { get; set; } = "410 Westinghouse Blvd, Suite B";
    public string City { get; set; } = "Charlotte";
    public string Region { get; set; } = "NC";
    public string PostalCode { get; set; } = "28273";
    public string Country { get; set; } = "US";
    public double Latitude { get; set; } = 35.1367;
    public double Longitude { get; set; } = -80.9339;
}
