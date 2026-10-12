namespace CarolinasCoverage.Site;

public sealed class SeoPage
{
    public string Title { get; init; } = "";
    public string Description { get; init; } = "";
    public string Canonical { get; init; } = "";
    public string AlternateEs { get; init; } = "";
    public string AlternateEn { get; init; } = "";
    public string Robots { get; init; } = "index, follow, max-image-preview:large, max-snippet:-1, max-video-preview:-1";
    public string OgType { get; init; } = "website";
    public string OgImage { get; init; } = "";
    public string OgImageAlt { get; init; } = "";
    public string Locale { get; init; } = "es_US";
    public string LocaleAlternate { get; init; } = "en_US";
    public IReadOnlyList<object> JsonLd { get; init; } = [];
    public bool English { get; init; }
}

public sealed record FaqItem(string Question, string Answer);

public sealed record Crumb(string Name, string Url);
