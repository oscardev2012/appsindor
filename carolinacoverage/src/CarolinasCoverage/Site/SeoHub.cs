namespace CarolinasCoverage.Site;

public sealed record HubSection(string H2En, string H2Es, string BodyEn, string BodyEs);

public sealed class HubDef
{
    public required string Id { get; init; }
    public required string SlugEn { get; init; }
    public required string SlugEs { get; init; }
    public required string H1En { get; init; }
    public required string H1Es { get; init; }
    public required string TitleEn { get; init; }
    public required string TitleEs { get; init; }
    public required string DescEn { get; init; }
    public required string DescEs { get; init; }
    public required string IntroEn { get; init; }
    public required string IntroEs { get; init; }
    public string Icon { get; init; } = "●";
    public string EyebrowEn { get; init; } = "Carolinas Coverage";
    public string EyebrowEs { get; init; } = "Carolinas Coverage";
    public string CtaEn { get; init; } = "Request a bilingual quote";
    public string CtaEs { get; init; } = "Solicitar cotización bilingüe";
    public string QuoteProduct { get; init; } = "";
    public string ParentId { get; init; } = "";
    public string FaqKey { get; init; } = "";
    public string Schema { get; init; } = "Service";
    public IReadOnlyList<string> RelatedIds { get; init; } = [];
    public IReadOnlyList<HubSection> Sections { get; init; } = [];
    public string NoteEn { get; init; } = "";
    public string NoteEs { get; init; } = "";
    public string Priority { get; init; } = "0.8";

    public string Path(bool en) => en ? SlugEn : SlugEs;
}
