using CalentonesAgua.Data;

namespace CalentonesAgua.Models;

public sealed class SeoPage
{
    public string Priority { get; init; } = "P1";
    public string Language { get; init; } = "en";
    public string Slug { get; init; } = "/";
    public string H1 { get; init; } = "";
    public string Title { get; init; } = "";
    public string Description { get; init; } = "";
    public string Cluster { get; init; } = "";
    public string Intent { get; init; } = "";
    public string Eyebrow { get; init; } = "";
    public string AnswerFirst { get; init; } = "";
    public string CtaLabel { get; init; } = "Text photos to SAFE";
    public string CtaHref { get; init; } = SiteContact.TextHref;
    public string SecondaryCtaLabel { get; init; } = "Call " + SiteContact.CallDisplay;
    public string SecondaryCtaHref { get; init; } = SiteContact.CallHref;
    public bool ShowSafetyBar { get; init; }
    public bool ShowPricing { get; init; }
    public bool IsQuotePage { get; init; }
    public string Author { get; init; } = "SAFE Water Heater technical team";
    public DateTime Reviewed { get; init; } = new(2026, 8, 29);
    public string? AlternateLangSlug { get; init; }
    public IReadOnlyList<SeoSection> Sections { get; init; } = [];
    public IReadOnlyList<SeoFaq> Faqs { get; init; } = [];
    public IReadOnlyList<SeoRelatedLink> Related { get; init; } = [];
}

public sealed class SeoSection
{
    public string Heading { get; init; } = "";
    public string Body { get; init; } = "";
    public IReadOnlyList<string>? Bullets { get; init; }
}

public sealed class SeoFaq
{
    public string Question { get; init; } = "";
    public string Answer { get; init; } = "";
}

public sealed class SeoRelatedLink
{
    public string Slug { get; init; } = "/";
    public string Label { get; init; } = "";
}

public sealed class SeoQuoteViewModel
{
    [System.ComponentModel.DataAnnotations.Required, System.ComponentModel.DataAnnotations.Display(Name = "Full name")]
    public string Name { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Required, System.ComponentModel.DataAnnotations.Display(Name = "Mobile phone")]
    public string Phone { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.EmailAddress]
    public string? Email { get; set; }

    [System.ComponentModel.DataAnnotations.Display(Name = "Property address")]
    public string? Address { get; set; }

    [System.ComponentModel.DataAnnotations.Required]
    public string City { get; set; } = "Charlotte";

    [System.ComponentModel.DataAnnotations.Display(Name = "Fuel type")]
    public string? Fuel { get; set; }

    [System.ComponentModel.DataAnnotations.Display(Name = "Tank size")]
    public string? TankSize { get; set; }

    [System.ComponentModel.DataAnnotations.Display(Name = "What is happening")]
    public string? Issue { get; set; }

    /// <summary>Optional SMS opt-in. Unchecked by default. Not required to submit the quote.</summary>
    public bool SmsConsent { get; set; }

    public string? UtmSource { get; set; }
    public string? UtmCampaign { get; set; }
    public string? Gclid { get; set; }
    public string? LandingPath { get; set; }

    /// <summary>Honeypot. Must stay empty. Bots that fill it are not saved.</summary>
    public string? CompanyWebsite { get; set; }
}
