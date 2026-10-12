using System.Text.Encodings.Web;
using System.Text.Json;
using CalentonesAgua.Models;

namespace CalentonesAgua.Data;

public static class SeoSchema
{
    public static readonly string[] ServiceCities =
    [
        "Charlotte, NC", "Shelby, NC", "China Grove, NC", "Mint Hill, NC", "Gastonia, NC",
        "Kings Mountain, NC", "Cherryville, NC", "Belmont, NC", "Mount Holly, NC",
        "Cornelius, NC", "Huntersville, NC", "Concord, NC", "Kannapolis, NC",
        "Salisbury, NC", "Mooresville, NC", "Matthews, NC", "Rock Hill, SC"
    ];

    public static IReadOnlyList<SeoTopicGroup> TopicGroups { get; } =
    [
        new("Local replacement", "Reemplazo local",
            ["/water-heater-replacement-charlotte-nc/", "/water-heater-service-near-me/", "/es/calentadores-de-agua-charlotte-nc/", "/es/reemplazo-calentador-agua-charlotte-nc/"]),
        new("Price and quotes", "Precio y cotización",
            ["/water-heater-cost-charlotte-nc/", "/water-heater-replacement-quote/", "/water-heater-financing/", "/water-heater-code-upgrades/", "/water-heater-permit-charlotte-nc/"]),
        new("Choose a unit", "Elegir equipo",
            ["/40-gallon-electric-water-heater-installation/", "/50-gallon-water-heater-installation/", "/what-size-water-heater-do-i-need/", "/water-heater-buying-guide/", "/gas-water-heater-installation/", "/tankless-water-heater-installation/", "/heat-pump-water-heater-installation/"]),
        new("Urgent help", "Urgencia",
            ["/water-heater-emergency/", "/same-day-water-heater-replacement/", "/water-heater-leaking/", "/water-heater-leak-damage/"]),
        new("Diagnosis", "Diagnóstico",
            ["/no-hot-water/", "/electric-water-heater-not-working/", "/gas-water-heater-not-working/", "/not-enough-hot-water/", "/water-heater-making-noise/", "/smelly-discolored-hot-water/", "/low-hot-water-pressure/", "/tankless-water-heater-problems/", "/new-water-heater-not-working/"]),
        new("Repair or replace", "Reparar o reemplazar",
            ["/repair-or-replace-water-heater/", "/water-heater-installation-process/", "/attic-water-heater-installation/", "/difficult-access-water-heater-installation/", "/water-heater-special-installations/"]),
        new("Warranty and care", "Garantía y cuidado",
            ["/water-heater-insurance-warranty/", "/water-heater-maintenance/"]),
        new("Pros and property managers", "Realtors y property managers",
            ["/realtor-water-heater-services/", "/property-manager-water-heater-program/"])
    ];

    public const string OrganizationDescription =
        "HOME INDOR TECHNOLOGY INC (also known as Safe Project Solution) operates SAFE Water Heater, a Charlotte, NC water-heater replacement specialist serving the Charlotte metro including Shelby, China Grove and Mint Hill. Published installed pricing from $1,199 + tax. Text +1 (650) 669-6890. Call 704-839-6126.";

    public const string BrandDescription =
        "SAFE Water Heater is the water-heater specialty brand of HOME INDOR TECHNOLOGY INC, also known as Safe Project Solution. Like-for-like electric and gas tank replacement in Charlotte, NC and nearby cities. We do not advertise 24/7 coverage.";

    private static readonly JsonSerializerOptions JsonLdOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    /// <summary>
    /// Standalone Organization JSON-LD (not wrapped in @graph) so naive parsers
    /// that read json.name / json.description on the first script tag still succeed.
    /// </summary>
    public static string OrganizationCard(string origin)
    {
        origin = origin.TrimEnd('/');
        return JsonSerializer.Serialize(Organization(origin, origin + "/#organization", origin + "/#business", includeContext: true), JsonLdOptions);
    }

    public static string FaqHub(string origin, IReadOnlyList<SeoFaq> faqs)
    {
        origin = origin.TrimEnd('/');
        var page = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "FAQPage",
            ["@id"] = origin + "/faqs#faq",
            ["url"] = origin + "/faqs",
            ["name"] = "Water heater FAQs for Charlotte NC — SAFE Water Heater",
            ["inLanguage"] = "en-US",
            ["mainEntity"] = faqs.Select(f => new Dictionary<string, object?>
            {
                ["@type"] = "Question",
                ["name"] = f.Question,
                ["acceptedAnswer"] = new Dictionary<string, string>
                {
                    ["@type"] = "Answer",
                    ["text"] = f.Answer
                }
            }).ToList()
        };
        return JsonSerializer.Serialize(page, JsonLdOptions);
    }

    public static string Graph(string origin, SeoPage? page = null, string? extraTitle = null, string? pageUrl = null, string? extraDescription = null)
    {
        var businessId = origin + "/#business";
        var orgId = origin + "/#organization";
        var websiteId = origin + "/#website";

        var graph = new List<object>
        {
            Organization(origin, orgId, businessId),
            LocalBusiness(origin, businessId, orgId),
            WebSite(origin, websiteId, orgId)
        };

        if (page is not null)
        {
            graph.Add(WebPage(origin, page, businessId));
            graph.Add(Breadcrumbs(origin, page));
            if (page.Faqs.Count > 0)
            {
                graph.Add(FaqPage(page));
            }

            if (page.ShowPricing)
            {
                graph.Add(OfferCatalog(origin, businessId));
            }

            if (page.Slug == "/water-heater-emergency/")
            {
                graph.Add(EmergencyHowTo());
            }

            if (page.Slug == "/water-heater-installation-process/")
            {
                graph.Add(InstallHowTo());
            }
        }
        else if (!string.IsNullOrEmpty(extraTitle))
        {
            graph.Add(new Dictionary<string, object?>
            {
                ["@type"] = "WebPage",
                ["name"] = extraTitle,
                ["description"] = extraDescription ?? BrandDescription,
                ["url"] = origin + (pageUrl ?? "/"),
                ["isPartOf"] = new Dictionary<string, string> { ["@id"] = websiteId },
                ["about"] = new Dictionary<string, string> { ["@id"] = businessId }
            });
        }

        return JsonSerializer.Serialize(new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@graph"] = graph
        }, JsonLdOptions);
    }

    private static Dictionary<string, object?> Organization(string origin, string orgId, string businessId, bool includeContext = false)
    {
        origin = origin.TrimEnd('/');
        var node = new Dictionary<string, object?>
        {
            ["@type"] = "Organization",
            ["@id"] = orgId,
            ["name"] = "SAFE Water Heater",
            ["legalName"] = "HOME INDOR TECHNOLOGY INC",
            ["alternateName"] = new[] { "Safe Project Solution", "HOME INDOR TECHNOLOGY INC" },
            ["description"] = OrganizationDescription,
            ["disambiguatingDescription"] = "Charlotte, North Carolina water heater replacement company (HOME INDOR TECHNOLOGY INC), not a generic safety product or national plumbing-parts brand.",
            ["url"] = origin + "/",
            ["logo"] = origin + "/safe-shield.svg",
            ["email"] = "safeprojectsolution@gmail.com",
            ["telephone"] = "+17048396126",
            ["address"] = new Dictionary<string, string>
            {
                ["@type"] = "PostalAddress",
                ["addressLocality"] = "Charlotte",
                ["addressRegion"] = "NC",
                ["addressCountry"] = "US"
            },
            ["contactPoint"] = new Dictionary<string, object?>
            {
                ["@type"] = "ContactPoint",
                ["telephone"] = "+17048396126",
                ["email"] = "safeprojectsolution@gmail.com",
                ["contactType"] = "customer service",
                ["areaServed"] = "US-NC",
                ["availableLanguage"] = new[] { "English", "Spanish" }
            },
            ["brand"] = new Dictionary<string, string>
            {
                ["@type"] = "Brand",
                ["name"] = "SAFE Water Heater"
            },
            ["department"] = new Dictionary<string, string> { ["@id"] = businessId }
        };
        if (includeContext)
        {
            node["@context"] = "https://schema.org";
        }

        return node;
    }

    private static Dictionary<string, object?> LocalBusiness(string origin, string businessId, string orgId) => new()
    {
        ["@type"] = new[] { "HVACBusiness", "Plumber" },
        ["@id"] = businessId,
        ["name"] = "SAFE Water Heater",
        ["legalName"] = "HOME INDOR TECHNOLOGY INC",
        ["alternateName"] = new[] { "Safe Project Solution", "HOME INDOR TECHNOLOGY INC" },
        ["description"] = BrandDescription,
        ["disambiguatingDescription"] = "Water heater replacement specialists in Charlotte, NC, operated by HOME INDOR TECHNOLOGY INC.",
        ["url"] = origin + "/",
        ["brand"] = new Dictionary<string, string>
        {
            ["@type"] = "Brand",
            ["name"] = "SAFE Water Heater"
        },
        ["image"] = origin + "/safe-electric-water-heater-hero.png",
        ["telephone"] = "+17048396126",
        ["email"] = "safeprojectsolution@gmail.com",
        ["priceRange"] = "$1,199+",
        ["currenciesAccepted"] = "USD",
        ["paymentAccepted"] = "Cash, SAFE Homeowner Plan, Synchrony",
        ["parentOrganization"] = new Dictionary<string, string> { ["@id"] = orgId },
        ["contactPoint"] = new Dictionary<string, object?>
        {
            ["@type"] = "ContactPoint",
            ["telephone"] = "+17048396126",
            ["email"] = "safeprojectsolution@gmail.com",
            ["contactType"] = "customer service",
            ["areaServed"] = "US-NC",
            ["availableLanguage"] = new[] { "English", "Spanish" }
        },
        ["address"] = new Dictionary<string, string>
        {
            ["@type"] = "PostalAddress",
            ["addressLocality"] = "Charlotte",
            ["addressRegion"] = "NC",
            ["addressCountry"] = "US"
        },
        ["geo"] = new Dictionary<string, object>
        {
            ["@type"] = "GeoCoordinates",
            ["latitude"] = 35.2271,
            ["longitude"] = -80.8431
        },
        ["areaServed"] = ServiceCities.Select(city => new Dictionary<string, string>
        {
            ["@type"] = "City",
            ["name"] = city
        }).ToList(),
        ["knowsLanguage"] = new[] { "en", "es" },
        ["hasOfferCatalog"] = OfferCatalog(origin, businessId)
    };

    private static Dictionary<string, object?> WebSite(string origin, string websiteId, string orgId) => new()
    {
        ["@type"] = "WebSite",
        ["@id"] = websiteId,
        ["url"] = origin + "/",
        ["name"] = "SAFE Water Heater",
        ["alternateName"] = "safewaterheaterclt.com",
        ["description"] = BrandDescription,
        ["inLanguage"] = new[] { "en", "es" },
        ["publisher"] = new Dictionary<string, string> { ["@id"] = orgId }
    };

    private static Dictionary<string, object?> WebPage(string origin, SeoPage page, string businessId) => new()
    {
        ["@type"] = "WebPage",
        ["@id"] = origin + page.Slug,
        ["url"] = origin + page.Slug,
        ["name"] = page.H1,
        ["description"] = page.Description,
        ["dateModified"] = page.Reviewed.ToString("yyyy-MM-dd"),
        ["inLanguage"] = page.Language.Equals("es", StringComparison.OrdinalIgnoreCase) ? "es" : "en",
        ["isPartOf"] = new Dictionary<string, string> { ["@id"] = origin + "/#website" },
        ["about"] = new Dictionary<string, string> { ["@id"] = businessId },
        ["primaryImageOfPage"] = origin + "/safe-electric-water-heater-hero.png",
        ["author"] = new Dictionary<string, string>
        {
            ["@type"] = "Organization",
            ["name"] = page.Author,
            ["@id"] = origin + "/#organization"
        }
    };

    private static Dictionary<string, object?> Breadcrumbs(string origin, SeoPage page) => new()
    {
        ["@type"] = "BreadcrumbList",
        ["itemListElement"] = new object[]
        {
            new Dictionary<string, object?>
            {
                ["@type"] = "ListItem",
                ["position"] = 1,
                ["name"] = "Home",
                ["item"] = origin + "/"
            },
            new Dictionary<string, object?>
            {
                ["@type"] = "ListItem",
                ["position"] = 2,
                ["name"] = page.H1,
                ["item"] = origin + page.Slug
            }
        }
    };

    private static Dictionary<string, object?> FaqPage(SeoPage page) => new()
    {
        ["@type"] = "FAQPage",
        ["mainEntity"] = page.Faqs.Select(f => new Dictionary<string, object?>
        {
            ["@type"] = "Question",
            ["name"] = f.Question,
            ["acceptedAnswer"] = new Dictionary<string, string>
            {
                ["@type"] = "Answer",
                ["text"] = f.Answer
            }
        }).ToList()
    };

    private static Dictionary<string, object?> OfferCatalog(string origin, string businessId) => new()
    {
        ["@type"] = "OfferCatalog",
        ["name"] = "Installed water heater packages",
        ["itemListElement"] = new object[]
        {
            Offer(origin, businessId, "40-gallon electric water heater installed", 1199, "/40-gallon-electric-water-heater-installation/"),
            Offer(origin, businessId, "50-gallon electric water heater installed", 1349, "/50-gallon-water-heater-installation/"),
            Offer(origin, businessId, "40-gallon gas water heater installed", 1449, "/gas-water-heater-installation/", true)
        }
    };

    private static Dictionary<string, object?> Offer(string origin, string businessId, string name, decimal price, string slug, bool fromPrice = false) => new()
    {
        ["@type"] = "Offer",
        ["name"] = name,
        ["price"] = price,
        ["priceCurrency"] = "USD",
        ["priceSpecification"] = new Dictionary<string, object?>
        {
            ["@type"] = "UnitPriceSpecification",
            ["price"] = price,
            ["priceCurrency"] = "USD",
            ["valueAddedTaxIncluded"] = false,
            ["description"] = fromPrice ? "Starting price for a qualifying like-for-like replacement; extras quoted from photos." : "Plus applicable tax. Standard replacement using suitable existing connections."
        },
        ["availability"] = "https://schema.org/InStock",
        ["url"] = origin + slug,
        ["seller"] = new Dictionary<string, string> { ["@id"] = businessId }
    };

    private static Dictionary<string, object?> EmergencyHowTo() => new()
    {
        ["@type"] = "HowTo",
        ["name"] = "What to do in a water heater emergency",
        ["description"] = "Safe first steps for flooding, gas odor, carbon monoxide, sparks or a bulging tank in Charlotte-area homes.",
        ["step"] = new object[]
        {
            Step(1, "Stop flooding if it is safe", "Close the cold-water inlet without standing in water near electricity. Kill power only from a dry breaker."),
            Step(2, "Leave for gas or carbon monoxide", "If you smell gas or a CO alarm sounds, do not operate switches. Leave and call 911 or the gas utility."),
            Step(3, "Do not reset sparking or hot breakers", "Leave a sparking electric heater off and call a qualified technician."),
            Step(4, "Never cap a discharging T&P pipe", "Stay clear of hot discharge from a bulging or steaming tank, then arrange replacement.")
        }
    };

    private static Dictionary<string, object?> InstallHowTo() => new()
    {
        ["@type"] = "HowTo",
        ["name"] = "SAFE water heater installation process",
        ["totalTime"] = "PT4H",
        ["step"] = new object[]
        {
            Step(1, "Send photos", "Share the tank, data plate, connections and access path."),
            Step(2, "Confirm scope", "SAFE matches a published package or lists extras in writing."),
            Step(3, "Install and test", "A typical like-for-like swap takes about 2–4 hours with old-tank removal."),
            Step(4, "Register warranty", "Keep the serial number and SAFE labor warranty record.")
        }
    };

    private static Dictionary<string, object?> Step(int position, string name, string text) => new()
    {
        ["@type"] = "HowToStep",
        ["position"] = position,
        ["name"] = name,
        ["text"] = text
    };
}

public sealed record SeoTopicGroup(string En, string Es, IReadOnlyList<string> Slugs);

public sealed record SeoHubGroup(string Heading, IReadOnlyList<SeoPage> Pages);
