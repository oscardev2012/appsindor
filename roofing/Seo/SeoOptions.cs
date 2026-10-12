namespace MajesticRoofing.Seo;

public sealed class SeoOptions
{
    public const string SectionName = "Seo";

    public string SiteUrl { get; set; } = "";
    public string SiteName { get; set; } = "Majestic Roofing, Inc.";
    public string DefaultImage { get; set; } = "/projects/roof-project-11.jpeg";
    public string Phone { get; set; } = "+1-980-800-5991";
    public string PhoneDisplay { get; set; } = "(980) 800-5991";
    public string Email { get; set; } = "ericb.majestic@gmail.com";
    public string StreetAddress { get; set; } = "5122 Belstone Lane";
    public string Locality { get; set; } = "Charlotte";
    public string Region { get; set; } = "NC";
    public string PostalCode { get; set; } = "28215";
    public string Country { get; set; } = "US";
    public double Latitude { get; set; } = 35.2269;
    public double Longitude { get; set; } = -80.7311;
    public string GoogleSiteVerification { get; set; } = "";
    public string BingSiteVerification { get; set; } = "";
    public string Instagram { get; set; } = "https://www.instagram.com/majesticroofing1/";
    public string Facebook { get; set; } = "https://www.facebook.com/Majesticroofinginc/";
    public string BbbUrl { get; set; } = "https://www.bbb.org/us/nc/charlotte/profile/roofing-contractors/majestic-roofing-0473-92022094";
}
