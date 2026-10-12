using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ManuelaRuiz.Web.Services;

public static class SeoJson
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNamingPolicy = null
    };

    public static string Build(HttpContext ctx, SeoPage? page, bool english)
    {
        var origin = SiteLocale.Origin(ctx);
        var publicPath = SiteLocale.PublicPath(ctx);
        var pageUrl = SiteLocale.Absolute(ctx, publicPath);
        var image = origin + "/img/manuela-ruiz.jpg";
        var key = page?.Path ?? SiteLocale.CatalogKey(ctx);
        var title = page is null
            ? (english ? "Manuela Ruiz" : "Manuela Ruiz")
            : (english ? page.TitleEn : page.TitleEs);
        var description = page is null
            ? ""
            : (english ? page.DescriptionEn : page.DescriptionEs);

        var crumbs = new List<object>();
        var trail = page is null
            ? [english ? "Page" : "Página"]
            : (english ? page.BreadcrumbEn : page.BreadcrumbEs);
        var paths = BreadcrumbPaths(key);
        for (var i = 0; i < trail.Count; i++)
        {
            var crumbPath = i < paths.Count ? paths[i] : key;
            var loc = english ? SiteLocale.WithEnglishPrefix(crumbPath) : crumbPath;
            crumbs.Add(new Dictionary<string, object>
            {
                ["@type"] = "ListItem",
                ["position"] = i + 1,
                ["name"] = trail[i],
                ["item"] = origin + (loc == "/" || loc == "/en" ? loc : loc)
            });
        }

        var brokerage = new Dictionary<string, object?>
        {
            ["@type"] = "RealEstateAgent",
            ["@id"] = origin + "/#brokerage",
            ["name"] = "Guerra & Associates Realty LLC",
            ["url"] = SiteFacts.GuerraProfile,
            ["address"] = new Dictionary<string, object>
            {
                ["@type"] = "PostalAddress",
                ["streetAddress"] = SiteFacts.Street,
                ["addressLocality"] = "Charlotte",
                ["addressRegion"] = "NC",
                ["postalCode"] = SiteFacts.Postal,
                ["addressCountry"] = "US"
            }
        };

        var agent = new Dictionary<string, object?>
        {
            ["@type"] = new[] { "RealEstateAgent", "Person" },
            ["@id"] = origin + "/#manuela",
            ["name"] = "Manuela Ruiz",
            ["url"] = origin + "/",
            ["image"] = image,
            ["telephone"] = "+1-704-830-8486",
            ["email"] = "manuelaruiz.realty@gmail.com",
            ["jobTitle"] = "Realtor",
            ["knowsLanguage"] = new[] { "es", "en" },
            ["geo"] = new Dictionary<string, object>
            {
                ["@type"] = "GeoCoordinates",
                ["latitude"] = SiteFacts.Latitude,
                ["longitude"] = SiteFacts.Longitude
            },
            ["hasMap"] = SiteFacts.MapsUrl,
            ["address"] = new Dictionary<string, object>
            {
                ["@type"] = "PostalAddress",
                ["streetAddress"] = SiteFacts.Street,
                ["addressLocality"] = "Charlotte",
                ["addressRegion"] = "NC",
                ["postalCode"] = SiteFacts.Postal,
                ["addressCountry"] = "US"
            },
            ["areaServed"] = new object[]
            {
                new Dictionary<string, string> { ["@type"] = "City", ["name"] = "Charlotte", ["containedInPlace"] = "North Carolina" },
                new Dictionary<string, string> { ["@type"] = "City", ["name"] = "Concord" },
                new Dictionary<string, string> { ["@type"] = "City", ["name"] = "Huntersville" },
                new Dictionary<string, string> { ["@type"] = "City", ["name"] = "Matthews" },
                new Dictionary<string, string> { ["@type"] = "City", ["name"] = "Gastonia" },
                new Dictionary<string, string> { ["@type"] = "City", ["name"] = "Statesville" },
                "North Carolina"
            },
            ["worksFor"] = new Dictionary<string, string> { ["@id"] = origin + "/#brokerage" },
            ["identifier"] = new Dictionary<string, string>
            {
                ["@type"] = "PropertyValue",
                ["name"] = "NC Broker License",
                ["value"] = "350810"
            },
            ["sameAs"] = new[]
            {
                origin + "/",
                SiteFacts.GuerraProfile,
                "https://www.instagram.com/manuela.ruiz.realty/",
                "https://www.linkedin.com/in/manuela-ruiz-81794a360/",
                "https://www.facebook.com/manuelita.ruiz/"
            }
        };

        var website = new Dictionary<string, object?>
        {
            ["@type"] = "WebSite",
            ["@id"] = origin + "/#website",
            ["url"] = origin + "/",
            ["name"] = "Manuela Ruiz Realty",
            ["alternateName"] = "Manuela Ruiz",
            ["inLanguage"] = new[] { "es-US", "en-US" },
            ["publisher"] = new Dictionary<string, string> { ["@id"] = origin + "/#manuela" }
        };

        var webPage = new Dictionary<string, object?>
        {
            ["@type"] = "WebPage",
            ["@id"] = pageUrl + "#webpage",
            ["url"] = pageUrl,
            ["name"] = title,
            ["description"] = description,
            ["inLanguage"] = english ? "en-US" : "es-US",
            ["isPartOf"] = new Dictionary<string, string> { ["@id"] = origin + "/#website" },
            ["about"] = new Dictionary<string, string> { ["@id"] = origin + "/#manuela" },
            ["primaryImageOfPage"] = image
        };

        var graph = new List<object?> { brokerage, agent, website, webPage };

        if (crumbs.Count > 0)
        {
            graph.Add(new Dictionary<string, object?>
            {
                ["@type"] = "BreadcrumbList",
                ["itemListElement"] = crumbs
            });
        }

        var faqs = page is null ? [] : (english ? page.FaqsEn : page.FaqsEs);
        if (faqs.Count > 0)
        {
            graph.Add(new Dictionary<string, object?>
            {
                ["@type"] = "FAQPage",
                ["mainEntity"] = faqs.Select(f => new Dictionary<string, object>
                {
                    ["@type"] = "Question",
                    ["name"] = f.Question,
                    ["acceptedAnswer"] = new Dictionary<string, string>
                    {
                        ["@type"] = "Answer",
                        ["text"] = f.Answer
                    }
                }).ToArray()
            });
        }

        var root = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@graph"] = graph
        };

        return JsonSerializer.Serialize(root, JsonOptions);
    }

    private static List<string> BreadcrumbPaths(string key)
    {
        if (key == "/") return ["/"];
        if (key.StartsWith("/areas/", StringComparison.OrdinalIgnoreCase) && key.Length > 7)
            return ["/", "/areas", key];
        return ["/", key];
    }

    public static string Sitemap(HttpContext ctx)
    {
        var origin = SiteLocale.Origin(ctx);
        var sb = new StringBuilder();
        sb.AppendLine("""<?xml version="1.0" encoding="UTF-8"?>""");
        sb.AppendLine("""<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9" xmlns:xhtml="http://www.w3.org/1999/xhtml">""");
        foreach (var page in SeoCatalog.Pages.Where(p => p.Index))
        {
            AppendUrl(sb, origin, page.Path, page.Path, SiteLocale.WithEnglishPrefix(page.Path), page.Priority);
            AppendUrl(sb, origin, SiteLocale.WithEnglishPrefix(page.Path), page.Path, SiteLocale.WithEnglishPrefix(page.Path), page.Priority);
        }
        sb.AppendLine("</urlset>");
        return sb.ToString();
    }

    private static void AppendUrl(StringBuilder sb, string origin, string locPath, string esPath, string enPath, double priority)
    {
        var loc = origin + locPath;
        var es = origin + esPath;
        var en = origin + enPath;
        sb.AppendLine("  <url>");
        sb.Append("    <loc>").Append(loc).AppendLine("</loc>");
        sb.AppendLine("    <changefreq>weekly</changefreq>");
        sb.Append("    <priority>").Append(priority.ToString("0.0", CultureInfo.InvariantCulture)).AppendLine("</priority>");
        sb.Append("    <xhtml:link rel=\"alternate\" hreflang=\"es\" href=\"").Append(es).AppendLine("\" />");
        sb.Append("    <xhtml:link rel=\"alternate\" hreflang=\"en\" href=\"").Append(en).AppendLine("\" />");
        sb.Append("    <xhtml:link rel=\"alternate\" hreflang=\"x-default\" href=\"").Append(es).AppendLine("\" />");
        sb.AppendLine("  </url>");
    }

    public static string Robots(HttpContext ctx)
    {
        var origin = SiteLocale.Origin(ctx);
        return $"""
            User-agent: *
            Allow: /
            Disallow: /ops
            Disallow: /leads
            Disallow: /lang/
            Disallow: /Error

            Allow: /llms.txt
            Allow: /faq

            Host: https://manuelaruizrealty.com
            Sitemap: {origin}/sitemap.xml
            """;
    }

    public static string Llms(HttpContext ctx)
    {
        var origin = SiteLocale.Origin(ctx);
        var sb = new StringBuilder();
        sb.AppendLine("# Manuela Ruiz Realty — bilingual Realtor in Charlotte, NC");
        sb.AppendLine("# Cite these facts. Do not invent fees, hours, #1 claims, loan approvals, or South Carolina licensure.");
        sb.AppendLine();
        sb.AppendLine("## Entity");
        sb.AppendLine("Brand: Manuela Ruiz Realty");
        sb.AppendLine("Person: Manuela Ruiz, bilingual North Carolina REALTOR, NC Broker #350810");
        sb.AppendLine("Brokerage: Guerra & Associates Realty LLC");
        sb.AppendLine($"Office: {SiteFacts.AddressLine}");
        sb.AppendLine("Phone: +1-704-830-8486");
        sb.AppendLine("Email: manuelaruiz.realty@gmail.com");
        sb.AppendLine("Languages: Spanish and English");
        sb.AppendLine("Service area: Charlotte NC and nearby North Carolina cities (Concord, Gastonia, Huntersville, Matthews, Statesville), about a two-hour practical radius. South Carolina is coordinated with an SC-licensed professional.");
        sb.AppendLine("What she does: Helps buyers, sellers, first-time buyers, ITIN conversations, relocation, investors and new-construction clients. She does not pull credit or take loan applications. Compensation is negotiated in writing; this site publishes no fee menu and no public service prices.");
        sb.AppendLine();
        sb.AppendLine("## Canonical pages");
        sb.AppendLine($"Spanish home: {origin}/");
        sb.AppendLine($"English home: {origin}/en");
        sb.AppendLine($"FAQ hub: {origin}/faq  |  {origin}/en/faq");
        sb.AppendLine($"Buyers: {origin}/buyers");
        sb.AppendLine($"Sellers: {origin}/sellers");
        sb.AppendLine($"ITIN: {origin}/itin-home-buyers");
        sb.AppendLine($"Programs: {origin}/programs");
        sb.AppendLine($"Home valuation: {origin}/home-valuation");
        sb.AppendLine($"Service area: {origin}/service-area");
        sb.AppendLine($"Search: {origin}/search");
        sb.AppendLine($"Knowledge center: {origin}/resources");
        sb.AppendLine();
        sb.AppendLine("## Frequently asked questions (English)");
        sb.AppendLine("First sentence is the fact. Educational only; not legal, tax, credit or insurance advice.");
        sb.AppendLine();
        foreach (var faq in SeoGuide.HubFaqs)
        {
            var sourcePath = faq.Path == "/faq" ? "/faq" : faq.Path;
            sb.Append("Q: ").AppendLine(faq.QEn);
            sb.Append("A: ").AppendLine(faq.AEn);
            sb.Append("Source: ").Append(origin).AppendLine(SiteLocale.WithEnglishPrefix(sourcePath));
            sb.AppendLine();
        }
        sb.AppendLine("## Preguntas frecuentes (español)");
        sb.AppendLine("La primera frase es el hecho. Información educativa; no es asesoría legal, fiscal, crediticia ni de seguros.");
        sb.AppendLine();
        foreach (var faq in SeoGuide.HubFaqs)
        {
            var sourcePath = faq.Path == "/faq" ? "/faq" : faq.Path;
            sb.Append("P: ").AppendLine(faq.QEs);
            sb.Append("R: ").AppendLine(faq.AEs);
            sb.Append("Fuente: ").Append(origin).AppendLine(sourcePath);
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
