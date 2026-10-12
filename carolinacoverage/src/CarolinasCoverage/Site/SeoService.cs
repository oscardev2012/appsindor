using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace CarolinasCoverage.Site;

public sealed class SeoService
{
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly SiteSettings _site;
    private readonly SiteCatalog _catalog;
    private readonly IHttpContextAccessor _http;

    public SeoService(IOptions<SiteSettings> site, SiteCatalog catalog, IHttpContextAccessor http)
    {
        _site = site.Value;
        _catalog = catalog;
        _http = http;
    }

    public string BaseUrl
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(_site.PublicBaseUrl))
            {
                return _site.PublicBaseUrl.TrimEnd('/');
            }

            var request = _http.HttpContext?.Request;
            if (request is null)
            {
                return "https://carolinascoverageinsurance.com";
            }

            var host = request.Host.Host;
            if (host.Equals("www.carolinascoverageinsurance.com", StringComparison.OrdinalIgnoreCase)
                || host.Equals("carolinascoverageinsurance.com", StringComparison.OrdinalIgnoreCase))
            {
                return "https://carolinascoverageinsurance.com";
            }

            return $"{request.Scheme}://{request.Host.Value}".TrimEnd('/');
        }
    }

    public string Absolute(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || path == "/")
        {
            return BaseUrl + "/";
        }

        if (!path.StartsWith('/'))
        {
            path = "/" + path;
        }

        return BaseUrl + path;
    }

    public string ImageUrl => Absolute("/images/carolinas-hero-family.png");

    public SeoPage Home(bool en)
    {
        var canonical = Absolute(en ? "/" : "/es");
        var title = en
            ? "Insurance Agency Serving NC & SC | Carolinas Coverage"
            : "Agencia de seguros NC y SC | Carolinas Coverage";
        var description = en
            ? "Insurance agency serving North Carolina and South Carolina. Auto, home, renters, pet, business and life. Bilingual quotes. Charlotte office. Call 704-595-3429."
            : "Agencia de seguros para Carolina del Norte y Carolina del Sur. Auto, casa, inquilinos, mascotas, negocio y vida. Cotización bilingüe. Oficina en Charlotte. 704-595-3429.";
        return Build(
            en,
            canonical,
            Absolute("/es"),
            Absolute("/"),
            title,
            description,
            "index, follow, max-image-preview:large, max-snippet:-1, max-video-preview:-1",
            AgencyGraph(en, canonical, title, description, en
                ? "Insurance Agency Serving North Carolina & South Carolina"
                : "Agencia de Seguros para Carolina del Norte y Carolina del Sur",
                FaqCatalog.Home(en)));
    }

    public SeoPage Hub(bool en, HubDef hub, IReadOnlyList<FaqItem> faqs, IReadOnlyList<Crumb> crumbs)
    {
        var canonical = Absolute(hub.Path(en));
        var title = en ? hub.TitleEn : hub.TitleEs;
        var description = en ? hub.DescEn : hub.DescEs;
        var graph = new List<object>
        {
            AgencyNode(en),
            WebPageNode(en, canonical, title, description, en ? hub.H1En : hub.H1Es, hub.Schema),
            BreadcrumbNode(crumbs)
        };
        if (faqs.Count > 0)
        {
            graph.Add(FaqNode(faqs));
        }

        return Build(en, canonical, Absolute(hub.SlugEs), Absolute(hub.SlugEn), title, description,
            "index, follow, max-image-preview:large, max-snippet:-1", graph);
    }

    public SeoPage Faqs(bool en)
    {
        var canonical = Absolute(FaqHubCatalog.Path(en));
        var title = en
            ? "Insurance FAQs for NC & SC | Carolinas Coverage"
            : "Preguntas frecuentes de seguros NC y SC | Carolinas Coverage";
        var description = en
            ? "Answer-first FAQs for auto, home, business, pet and life insurance in North Carolina and South Carolina. Charlotte office. Call 704-595-3429."
            : "Preguntas y respuestas de auto, casa, negocio, mascotas y vida en Carolina del Norte y Carolina del Sur. Oficina en Charlotte. 704-595-3429.";
        var crumbs = new[]
        {
            new Crumb(en ? "Home" : "Inicio", Lang.Home(en)),
            new Crumb(en ? "FAQs" : "Preguntas frecuentes", FaqHubCatalog.Path(en))
        };
        var faqs = FaqHubCatalog.ForSchema(en);
        var graph = new List<object>
        {
            AgencyNode(en),
            WebPageNode(en, canonical, title, description, en
                ? "Insurance FAQs for North Carolina and South Carolina"
                : "Preguntas frecuentes de seguros en Carolina del Norte y Carolina del Sur"),
            BreadcrumbNode(crumbs),
            FaqNode(faqs)
        };
        return Build(en, canonical, Absolute(FaqHubCatalog.PathEs), Absolute(FaqHubCatalog.PathEn), title, description,
            "index, follow, max-image-preview:large, max-snippet:-1", graph);
    }

    public SeoPage Guide(bool en, string productId, GuideProduct product, GuideLocale copy)
    {
        var path = Lang.Coverage(en, productId);
        var canonical = Absolute(path);
        var (title, description) = GuideMeta(en, productId, copy);
        var faqs = new[]
        {
            new FaqItem(en ? "What coverage is commonly discussed?" : "¿Qué coberturas se suelen conversar?", string.Join(" ", copy.Common)),
            new FaqItem(en ? "How does a claim commonly work?" : "¿Cómo suele funcionar un reclamo?", string.Join(" ", copy.Claim)),
            new FaqItem(en ? "What may an agent ask?" : "¿Qué puede preguntar un agente?", string.Join(" ", copy.Ask))
        };

        var graph = new List<object> { AgencyNode(en), WebPageNode(en, canonical, title, description, copy.Title), BreadcrumbNode(en, productId, copy), FaqNode(faqs) };
        var robots = SeoHubCatalog.IsUnconfirmedSpecialty(productId)
            ? "noindex, follow"
            : "index, follow, max-image-preview:large, max-snippet:-1";
        return Build(en, canonical, Absolute(Lang.Coverage(false, productId)), Absolute(Lang.Coverage(true, productId)), title, description, robots, graph);
    }

    public SeoPage Service(bool en, string title, string description)
    {
        var request = _http.HttpContext!.Request;
        var canonical = Absolute(en ? Lang.ToEnglish(request) : Lang.ToSpanish(request));
        return Build(
            en,
            canonical,
            Absolute(Lang.ToSpanish(request)),
            Absolute(Lang.ToEnglish(request)),
            $"{title} | { _site.Name}",
            description,
            "noindex, nofollow",
            [AgencyNode(en)]);
    }

    public SeoPage Portal(bool en)
    {
        var canonical = Absolute(en ? "/portal-demo" : "/es/portal-demo");
        return Build(
            en,
            canonical,
            Absolute("/es/portal-demo"),
            Absolute("/portal-demo"),
            en ? "Customer portal preview | Carolinas Coverage" : "Panel del cliente | Carolinas Coverage",
            en
                ? "Preview of how Carolinas Coverage clients will follow requests, documents, and next steps."
                : "Vista previa de cómo los clientes de Carolinas Coverage seguirán solicitudes, documentos y próximos pasos.",
            "noindex, nofollow",
            [AgencyNode(en)]);
    }

    public SeoPage Error(bool en, int statusCode)
    {
        var canonical = Absolute(en ? "/" : "/es");
        return Build(
            en,
            canonical,
            Absolute("/es"),
            Absolute("/"),
            statusCode == 404
                ? (en ? "Page not found | Carolinas Coverage" : "Página no encontrada | Carolinas Coverage")
                : (en ? "Error | Carolinas Coverage" : "Error | Carolinas Coverage"),
            en
                ? "The requested page is not available. Return to Carolinas Coverage for auto, home, and business insurance in Charlotte."
                : "La página solicitada no está disponible. Vuelve a Carolinas Coverage para seguro de auto, hogar y negocio en Charlotte.",
            "noindex, follow",
            [AgencyNode(en)]);
    }

    public IReadOnlyList<(string Path, string AlternateEs, string AlternateEn, string LastMod, string Changefreq, string Priority)> SitemapEntries()
    {
        var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var entries = new List<(string, string, string, string, string, string)>
        {
            ("/", "/es", "/", today, "weekly", "1.0"),
            ("/es", "/es", "/", today, "weekly", "1.0"),
            (FaqHubCatalog.PathEn, FaqHubCatalog.PathEs, FaqHubCatalog.PathEn, today, "weekly", "0.8"),
            (FaqHubCatalog.PathEs, FaqHubCatalog.PathEs, FaqHubCatalog.PathEn, today, "weekly", "0.8")
        };

        foreach (var hub in SeoHubCatalog.All)
        {
            entries.Add((hub.SlugEn, hub.SlugEs, hub.SlugEn, today, "weekly", hub.Priority));
            entries.Add((hub.SlugEs, hub.SlugEs, hub.SlugEn, today, "weekly", hub.Priority));
        }

        foreach (var id in _catalog.Guides.Keys)
        {
            if (SeoHubCatalog.HasProduct(id) || SeoHubCatalog.IsUnconfirmedSpecialty(id))
            {
                continue;
            }

            entries.Add(($"/coverage/{id}", $"/es/coverage/{id}", $"/coverage/{id}", today, "monthly", "0.4"));
            entries.Add(($"/es/coverage/{id}", $"/es/coverage/{id}", $"/coverage/{id}", today, "monthly", "0.4"));
        }

        return entries;
    }

    public string SitemapXml()
    {
        try
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("""<?xml version="1.0" encoding="UTF-8"?>""");
            sb.AppendLine("""<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9" xmlns:xhtml="http://www.w3.org/1999/xhtml">""");
            foreach (var entry in SitemapEntries())
            {
                var loc = Xml(Absolute(entry.Path));
                var es = Xml(Absolute(entry.AlternateEs));
                var en = Xml(Absolute(entry.AlternateEn));
                sb.AppendLine("  <url>");
                sb.AppendLine($"    <loc>{loc}</loc>");
                sb.AppendLine($"    <lastmod>{entry.LastMod}</lastmod>");
                sb.AppendLine($"    <changefreq>{entry.Changefreq}</changefreq>");
                sb.AppendLine($"    <priority>{entry.Priority}</priority>");
                sb.AppendLine($"    <xhtml:link rel=\"alternate\" hreflang=\"es\" href=\"{es}\"/>");
                sb.AppendLine($"    <xhtml:link rel=\"alternate\" hreflang=\"en\" href=\"{en}\"/>");
                sb.AppendLine($"    <xhtml:link rel=\"alternate\" hreflang=\"x-default\" href=\"{en}\"/>");
                sb.AppendLine("  </url>");
            }

            sb.AppendLine("</urlset>");
            return sb.ToString();
        }
        catch
        {
            var home = Xml(Absolute("/"));
            var es = Xml(Absolute("/es"));
            return $"""
                    <?xml version="1.0" encoding="UTF-8"?>
                    <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
                      <url><loc>{home}</loc></url>
                      <url><loc>{es}</loc></url>
                    </urlset>
                    """;
        }
    }

    public string RobotsTxt() =>
        $"""
         User-agent: *
         Allow: /
         Disallow: /portal-demo
         Disallow: /es/portal-demo
         Disallow: /service/
         Disallow: /es/service/
         Disallow: /Error
         Disallow: /es/Error

         User-agent: GPTBot
         Allow: /

         User-agent: ChatGPT-User
         Allow: /

         User-agent: PerplexityBot
         Allow: /

         User-agent: Google-Extended
         Allow: /

         Sitemap: {Absolute("/sitemap.xml")}
         """;

    public string LlmsTxt()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("# Carolinas Coverage Insurance Agency");
        sb.AppendLine();
        sb.AppendLine($"> Insurance agency at 410 Westinghouse Blvd, Suite B, Charlotte, NC 28273. Phone 704-595-3429. Helps with auto, home/property, renters, pet, business and life insurance in North Carolina and South Carolina. Does not offer health insurance, Medicare or disability insurance. No public prices. This site does not issue a binding quote. Confirm hours when calling.");
        sb.AppendLine();
        sb.AppendLine($"Site: {Absolute("/")}");
        sb.AppendLine($"Spanish: {Absolute("/es")}");
        sb.AppendLine($"FAQs: {Absolute(FaqHubCatalog.PathEn)}");
        sb.AppendLine($"FAQs (Spanish): {Absolute(FaqHubCatalog.PathEs)}");
        sb.AppendLine($"About: {Absolute("/about")}");
        sb.AppendLine($"Sitemap: {Absolute("/sitemap.xml")}");
        sb.AppendLine();
        sb.AppendLine("## Identity");
        sb.AppendLine("Legal name: Carolinas Coverage Insurance Agency. Not Carolina Coverage Group (Mooresville). Not Carolina Coverage Solutions. Does not sell Medicare, health or disability insurance.");
        sb.AppendLine();
        sb.AppendLine("## Do not invent");
        sb.AppendLine("- Do not claim 24/7, #1, cheapest, best, or all carriers.");
        sb.AppendLine("- Do not invent hours, license numbers, EINs, reviews or prices.");
        sb.AppendLine("- Confirmed conversation carriers include Progressive, Nationwide, Safeco / Liberty Mutual, Dairyland, National General, Travelers, Assurant America, JSA and Berkshire Hathaway. Availability is never guaranteed.");
        sb.AppendLine();
        sb.AppendLine("## FAQs");
        foreach (var item in FaqHubCatalog.All)
        {
            sb.AppendLine($"Q: {item.QEn}");
            sb.AppendLine($"A: {item.FactEn} {item.WhyEn}");
            sb.AppendLine($"Source: {Absolute(FaqHubCatalog.RelatedPath(item.RelatedId, true))}");
            sb.AppendLine();
        }

        return sb.ToString();
    }

    public void Apply(Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary viewData, SeoPage seo)
    {
        viewData["Seo"] = seo;
        viewData["Title"] = seo.Title;
        viewData["Description"] = seo.Description;
        viewData["Lang"] = seo.English ? "en" : "es";
    }

    public string JsonLdHtml(SeoPage seo)
    {
        var blocks = seo.JsonLd.Select(node =>
            $"<script type=\"application/ld+json\">{JsonSerializer.Serialize(node, JsonOptions)}</script>");
        return string.Join('\n', blocks);
    }

    private SeoPage Build(bool en, string canonical, string es, string enUrl, string title, string description, string robots, IReadOnlyList<object> jsonLd) =>
        new()
        {
            English = en,
            Canonical = canonical,
            AlternateEs = es,
            AlternateEn = enUrl,
            Title = title,
            Description = description,
            Robots = robots,
            OgImage = ImageUrl,
            OgImageAlt = en
                ? "Family in front of their home and car — Carolinas Coverage Insurance Agency"
                : "Familia frente a su hogar y auto — Carolinas Coverage Insurance Agency",
            Locale = en ? "en_US" : "es_US",
            LocaleAlternate = en ? "es_US" : "en_US",
            JsonLd = jsonLd
        };

    private static (string Title, string Description) GuideMeta(bool en, string id, GuideLocale copy) => id switch
    {
        "auto" => en
            ? ("Auto Insurance in Charlotte, NC & SC | Carolinas Coverage",
                "Understand auto insurance in Charlotte and both Carolinas: liability, collision, comprehensive, and claims. Bilingual agents. Call 704-595-3429.")
            : ("Seguro de auto en Charlotte, NC y SC | Carolinas Coverage",
                "Entiende el seguro de auto en Charlotte y las dos Carolinas: responsabilidad, colisión, comprehensive y reclamos. Agentes bilingües. 704-595-3429."),
        "home" => en
            ? ("Home Insurance in Charlotte & the Carolinas | Carolinas Coverage",
                "A clear guide to homeowners insurance in Charlotte, Rock Hill, North Carolina, and South Carolina. Dwelling, belongings, liability, and claims.")
            : ("Seguro de hogar en Charlotte y las Carolinas | Carolinas Coverage",
                "Guía clara de seguro de hogar en Charlotte, Rock Hill, Carolina del Norte y Carolina del Sur. Vivienda, pertenencias, responsabilidad y reclamos."),
        "renters" => en
            ? ("Renters Insurance in Charlotte, NC | Carolinas Coverage",
                "Renters insurance for belongings and personal liability in Charlotte and nearby communities. Bilingual service. Call 704-595-3429.")
            : ("Seguro para inquilinos en Charlotte, NC | Carolinas Coverage",
                "Seguro para inquilinos: pertenencias y responsabilidad personal en Charlotte y alrededores. Atención bilingüe. Llama al 704-595-3429."),
        "pet" => en
            ? ("Pet Insurance Guide: Accident, Illness & Wellness | Carolinas Coverage",
                "Compare pet insurance basics: accident and illness plans, wellness, deductible, reimbursement, and annual limits. Charlotte bilingual agency.")
            : ("Seguro para mascotas: accidentes, enfermedad y bienestar | Carolinas Coverage",
                "Compara lo básico del seguro para mascotas: accidentes, enfermedades, bienestar, deducible, reembolso y límite anual. Agencia bilingüe en Charlotte."),
        "business" => en
            ? ("Business Insurance in Charlotte, NC | Carolinas Coverage",
                "General liability, workers’ compensation, commercial auto, and related business coverage in Charlotte and both Carolinas.")
            : ("Seguro para negocios en Charlotte, NC | Carolinas Coverage",
                "Responsabilidad civil, compensación laboral, auto comercial y coberturas de negocio en Charlotte y las dos Carolinas."),
        "commercial-auto" => en
            ? ("Commercial Auto Insurance in the Carolinas | Carolinas Coverage",
                "Commercial auto coverage for work vehicles, drivers, cargo, and DOT needs in North Carolina and South Carolina.")
            : ("Auto comercial en las Carolinas | Carolinas Coverage",
                "Seguro de auto comercial para vehículos de trabajo, conductores, carga y DOT en Carolina del Norte y Carolina del Sur."),
        "workers-comp" => en
            ? ("Workers’ Compensation Insurance | Carolinas Coverage",
                "A plain-language guide to workers’ compensation for businesses in North Carolina and South Carolina. Call 704-595-3429.")
            : ("Compensación laboral | Carolinas Coverage",
                "Guía clara de compensación laboral para negocios en Carolina del Norte y Carolina del Sur. Llama al 704-595-3429."),
        "general-liability" => en
            ? ("General Liability Insurance for Businesses | Carolinas Coverage",
                "Learn how general liability may respond to injury or third-party property damage claims. Charlotte bilingual insurance agency.")
            : ("Responsabilidad civil general para negocios | Carolinas Coverage",
                "Cómo la responsabilidad civil general puede responder a lesiones o daños a propiedad de terceros. Agencia bilingüe en Charlotte."),
        _ => (TrimTitle($"{ToTitleCase(copy.Label)} | Carolinas Coverage"), Truncate(copy.Intro, 158))
    };

    private List<object> AgencyGraph(bool en, string canonical, string title, string description, string headline, IReadOnlyList<FaqItem> faqs) =>
    [
        AgencyNode(en),
        WebSiteNode(),
        WebPageNode(en, canonical, title, description, headline, "InsuranceAgency"),
        FaqNode(faqs)
    ];

    private object AgencyNode(bool en) => new Dictionary<string, object?>
    {
        ["@context"] = "https://schema.org",
        ["@type"] = new[] { "InsuranceAgency", "LocalBusiness" },
        ["@id"] = Absolute("/#agency"),
        ["name"] = _site.Name,
        ["alternateName"] = new[] { "Carolinas Coverage", "Carolinas Coverage Insurance" },
        ["disambiguatingDescription"] = en
            ? "Independent insurance agency in Charlotte, NC. Not Carolina Coverage Group and not Carolina Coverage Solutions. Does not offer Medicare."
            : "Agencia de seguros independiente en Charlotte, NC. No es Carolina Coverage Group ni Carolina Coverage Solutions. No ofrece Medicare.",
        ["url"] = Absolute("/"),
        ["image"] = ImageUrl,
        ["logo"] = Absolute("/favicon.svg"),
        ["telephone"] = _site.Phone,
        ["inLanguage"] = new[] { "es", "en" },
        ["availableLanguage"] = new[] { "Spanish", "English" },
        ["areaServed"] = new object[]
        {
            new Dictionary<string, string> { ["@type"] = "City", ["name"] = "Charlotte" },
            new Dictionary<string, string> { ["@type"] = "City", ["name"] = "Rock Hill" },
            new Dictionary<string, string> { ["@type"] = "State", ["name"] = "North Carolina" },
            new Dictionary<string, string> { ["@type"] = "State", ["name"] = "South Carolina" }
        },
        ["address"] = new Dictionary<string, string>
        {
            ["@type"] = "PostalAddress",
            ["streetAddress"] = _site.Street,
            ["addressLocality"] = _site.City,
            ["addressRegion"] = _site.Region,
            ["postalCode"] = _site.PostalCode,
            ["addressCountry"] = _site.Country
        },
        ["geo"] = new Dictionary<string, object>
        {
            ["@type"] = "GeoCoordinates",
            ["latitude"] = _site.Latitude,
            ["longitude"] = _site.Longitude
        },
        ["hasMap"] = "https://maps.google.com/?q=410+Westinghouse+Blvd+Suite+B+Charlotte+NC+28273",
        ["knowsLanguage"] = new[] { "es", "en" },
        ["slogan"] = en ? "Clear insurance. Personal service." : "Seguro claro. Atención personal.",
        ["hasOfferCatalog"] = new Dictionary<string, object>
        {
            ["@type"] = "OfferCatalog",
            ["name"] = en ? "Insurance coverage options" : "Opciones de cobertura",
            ["itemListElement"] = _catalog.Products.Select(p => new Dictionary<string, object>
            {
                ["@type"] = "Offer",
                ["itemOffered"] = new Dictionary<string, object>
                {
                    ["@type"] = "Service",
                    ["name"] = en ? p.En : p.Es,
                    ["url"] = Absolute(Lang.Coverage(en, p.Id)),
                    ["description"] = en ? p.DescEn : p.DescEs,
                    ["provider"] = new Dictionary<string, string> { ["@id"] = Absolute("/#agency") }
                }
            }).ToArray()
        }
    };

    private object WebSiteNode() => new Dictionary<string, object>
    {
        ["@context"] = "https://schema.org",
        ["@type"] = "WebSite",
        ["@id"] = Absolute("/#website"),
        ["url"] = Absolute("/"),
        ["name"] = _site.Name,
        ["inLanguage"] = new[] { "es-US", "en-US" },
        ["publisher"] = new Dictionary<string, string> { ["@id"] = Absolute("/#agency") },
        ["mainEntity"] = new Dictionary<string, string> { ["@id"] = Absolute("/#agency") }
    };

    private object WebPageNode(bool en, string canonical, string title, string description, string headline, string schema = "WebPage")
    {
        var types = schema switch
        {
            "ContactPage" => new[] { "WebPage", "ContactPage" },
            "InsuranceAgency" => new[] { "WebPage" },
            _ => new[] { "WebPage" }
        };
        return new Dictionary<string, object>
    {
        ["@context"] = "https://schema.org",
        ["@type"] = types.Length == 1 ? types[0] : types,
        ["@id"] = canonical + "#webpage",
        ["url"] = canonical,
        ["name"] = title,
        ["headline"] = headline,
        ["description"] = description,
        ["inLanguage"] = en ? "en-US" : "es-US",
        ["isPartOf"] = new Dictionary<string, string> { ["@id"] = Absolute("/#website") },
        ["about"] = new Dictionary<string, string> { ["@id"] = Absolute("/#agency") },
        ["primaryImageOfPage"] = new Dictionary<string, object>
        {
            ["@type"] = "ImageObject",
            ["url"] = ImageUrl
        }
    };
    }

    private object BreadcrumbNode(IReadOnlyList<Crumb> crumbs) => new Dictionary<string, object>
    {
        ["@context"] = "https://schema.org",
        ["@type"] = "BreadcrumbList",
        ["itemListElement"] = crumbs.Select((crumb, index) => new Dictionary<string, object>
        {
            ["@type"] = "ListItem",
            ["position"] = index + 1,
            ["name"] = crumb.Name,
            ["item"] = Absolute(crumb.Url)
        }).ToArray()
    };

    private object BreadcrumbNode(bool en, string productId, GuideLocale copy) => new Dictionary<string, object>
    {
        ["@context"] = "https://schema.org",
        ["@type"] = "BreadcrumbList",
        ["itemListElement"] = new object[]
        {
            new Dictionary<string, object>
            {
                ["@type"] = "ListItem",
                ["position"] = 1,
                ["name"] = en ? "Home" : "Inicio",
                ["item"] = Absolute(Lang.Home(en))
            },
            new Dictionary<string, object>
            {
                ["@type"] = "ListItem",
                ["position"] = 2,
                ["name"] = en ? "Coverage" : "Coberturas",
                ["item"] = Absolute(Lang.Home(en) + "#products")
            },
            new Dictionary<string, object>
            {
                ["@type"] = "ListItem",
                ["position"] = 3,
                ["name"] = en ? ToTitleCase(copy.Label) : copy.Label,
                ["item"] = Absolute(Lang.Coverage(en, productId))
            }
        }
    };

    private static object FaqNode(IReadOnlyList<FaqItem> faqs) => new Dictionary<string, object>
    {
        ["@context"] = "https://schema.org",
        ["@type"] = "FAQPage",
        ["mainEntity"] = faqs.Select(item => new Dictionary<string, object>
        {
            ["@type"] = "Question",
            ["name"] = item.Question,
            ["acceptedAnswer"] = new Dictionary<string, string>
            {
                ["@type"] = "Answer",
                ["text"] = item.Answer
            }
        }).ToArray()
    };

    private static string Xml(string value) =>
        System.Security.SecurityElement.Escape(value) ?? "";

    private static string ToTitleCase(string value) =>
        string.Join(' ', value.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(w => char.ToUpperInvariant(w[0]) + w[1..]));

    private static string TrimTitle(string value) => value.Length <= 65 ? value : value[..62] + "…";

    private static string Truncate(string value, int max) =>
        value.Length <= max ? value : value[..(max - 1)].TrimEnd() + "…";
}
