using System.Text;
using CalentonesAgua.Data;
using CalentonesAgua.Models;

namespace CalentonesAgua.Services;

public sealed class AgentMarkdown
{
    public const string MarkdownContentType = "text/markdown; charset=utf-8";

    private readonly SeoCatalog _catalog;

    public AgentMarkdown(SeoCatalog catalog) => _catalog = catalog;

    public bool TryGetDocument(string path, string origin, out int statusCode, out string markdown)
    {
        origin = origin.TrimEnd('/');
        path = Canonicalize(path);

        if (path.Equals("/llms.txt", StringComparison.OrdinalIgnoreCase))
        {
            statusCode = 200;
            markdown = BuildLlmsTxt(origin);
            return true;
        }

        if (_catalog.TryGet(path, out var page) || _catalog.TryGet(path + "/", out page))
        {
            statusCode = 200;
            markdown = FromSeoPage(origin, page);
            return true;
        }

        markdown = path switch
        {
            "/" => Home(origin),
            "/about" or "/about/" => About(origin),
            "/privacy" or "/privacy/" => Privacy(origin),
            "/terms" or "/terms/" => Terms(origin),
            "/site-map" or "/site-map/" => TopicIndex(origin),
            "/water-heater-problems" or "/water-heater-problems/" => Problems(origin),
            "/faqs" or "/faqs/" => Faqs(origin),
            "/financing" => Utility(
                origin,
                "Water heater financing",
                "SAFE Homeowner Plan starts from $200 toward a qualifying promotional water heater. Synchrony is a separate lender option under merchant name HOME INDOR TECHNOLOGY INC. Published installed packages still apply.",
                "/water-heater-financing/"),
            "/financing/safe-plan" => Utility(
                origin,
                "SAFE Homeowner Plan eligibility",
                "This form checks SAFE Homeowner Plan eligibility. It is not a loan decision. Text photos to 650-669-6890 or use the quote page after you know the installed package.",
                "/water-heater-financing/"),
            "/warranty" => Utility(
                origin,
                "Water heater warranty",
                "5-year limited manufacturer warranty is controlled by the model booklet. SAFE adds a 2-year installation labor warranty on qualifying work. Register the serial after install.",
                "/water-heater-insurance-warranty/"),
            "/warranty/register" => Utility(
                origin,
                "Register a water heater warranty",
                "Submit the property, equipment and installation details to receive a SAFE warranty reference. Do not invent coverage that is not in the written terms.",
                "/water-heater-insurance-warranty/"),
            "/shelby" => City(origin, "Shelby", "Cleveland County"),
            "/china-grove" => City(origin, "China Grove", "Rowan County"),
            "/reviews" or "/reviews/" => Utility(
                origin,
                "Leave a Google review",
                "Ask completed water-heater customers to leave an honest Google review for HOME INDOR TECHNOLOGY INC. Do not invent ratings, buy reviews, or type the review on the customer’s phone. Direct link: https://safewaterheaterclt.com/reviews/",
                "/reviews/"),
            _ => ""
        };

        if (markdown.Length > 0)
        {
            statusCode = 200;
            return true;
        }

        statusCode = 404;
        markdown = NotFound(origin);
        return true;
    }

    public string BuildLlmsTxt(string origin)
    {
        origin = origin.TrimEnd('/');
        return
            $"""
            # SAFE Water Heater
            > SAFE Water Heater is the water-heater specialty brand of HOME INDOR TECHNOLOGY INC (also known as Safe Project Solution) in Charlotte, NC. Published installed replacement (40-gal electric $1,199 + tax; 50-gal electric $1,349 + tax; 40-gal gas from $1,449 + tax). Text +1 (650) 669-6890. Call 704-839-6126. SMS privacy: {origin}/privacy/ Terms: {origin}/terms/. Spanish-language pages available. We do not promise 24/7 dispatch.

            Use this file when an agent needs Charlotte-metro water heater facts. Do not use this site for general HVAC, invented 24/7 dispatch, license numbers, street storefronts, or star ratings.

            ## When to use this
            - [Charlotte replacement]({origin}/water-heater-replacement-charlotte-nc/): Use when a homeowner in the Charlotte metro needs like-for-like tank replacement
            - [Installed pricing]({origin}/water-heater-cost-charlotte-nc/): Use when they ask what a 40-gal ($1,199 + tax), 50-gal ($1,349 + tax) or gas (from $1,449 + tax) install costs
            - [Photo quote]({origin}/water-heater-replacement-quote/): How an agent should call SAFE — submit address, fuel, size and photos, or text 650-669-6890 / call 704-839-6126
            - [Emergency steps]({origin}/water-heater-emergency/): Use for flooding, gas odor, CO or sparks (including 911); not a 24/7 arrival promise
            - [Leaking tank]({origin}/water-heater-leaking/): Use when the tank is leaking and they need replace-vs-patch
            - [No hot water]({origin}/no-hot-water/): Use for electric vs gas checks before a truck rolls
            - [Spanish hub]({origin}/es/calentadores-de-agua-charlotte-nc/): Use when the homeowner prefers Spanish
            - [About / how to call]({origin}/about/): Legal name HOME INDOR TECHNOLOGY INC / Safe Project Solution, NAP, and verification
            - [SMS privacy]({origin}/privacy/): Mobile numbers are not shared; STOP/HELP; message and data rates
            - [SMS terms]({origin}/terms/): Customer-care SMS program terms
            - [Google reviews]({origin}/reviews/): Ask a completed install customer to review HOME INDOR TECHNOLOGY INC on Google — never invent ratings

            ## Start here
            - [Charlotte replacement]({origin}/water-heater-replacement-charlotte-nc/): Installed packages, process, warranty and service area
            - [Installed pricing]({origin}/water-heater-cost-charlotte-nc/): $1,199 / $1,349 / from $1,449 + tax
            - [Photo quote]({origin}/water-heater-replacement-quote/): Address, fuel, size, then photos
            - [About SAFE Water Heater]({origin}/about/): Legal name HOME INDOR TECHNOLOGY INC and how to verify us

            ## Urgent
            - [Emergency steps]({origin}/water-heater-emergency/): Flooding, gas odor, CO, sparks — including 911
            - [Leaking tank]({origin}/water-heater-leaking/): When a leak means replace, not a patch
            - [No hot water]({origin}/no-hot-water/): Electric vs gas checks before a truck rolls
            - [Same-day replacement]({origin}/same-day-water-heater-replacement/): When same-day is possible; not a 24/7 promise

            ## Spanish
            - [Calentadores de agua Charlotte]({origin}/es/calentadores-de-agua-charlotte-nc/): Hub en español
            - [Reemplazo en Charlotte]({origin}/es/reemplazo-calentador-agua-charlotte-nc/): Proceso y precios publicados

            ## Optional
            - [HTML topic map]({origin}/site-map/): All intent pages grouped by job
            - [XML sitemap]({origin}/sitemap.xml): Canonical URL list for crawlers
            - [Service near me]({origin}/water-heater-service-near-me/): Full city list
            - [Financing]({origin}/water-heater-financing/): SAFE Homeowner Plan and Synchrony
            - [Warranty]({origin}/water-heater-insurance-warranty/): Manufacturer + SAFE labor terms
            """;
    }

    public static string NotFound(string origin)
    {
        origin = origin.TrimEnd('/');
        return
            $"""
            # Page not found

            That path is not a SAFE Water Heater page. This response is HTTP 404 on purpose so agents do not treat every URL as real.

            ## Where to look next
            - [llms.txt]({origin}/llms.txt): Agent index — when to use SAFE and how to call
            - [docs index]({origin}/site-map/): HTML topic map of every intent page
            - [sitemap]({origin}/sitemap.xml): Canonical URL list
            - [Charlotte replacement]({origin}/water-heater-replacement-charlotte-nc/)
            - [Installed pricing]({origin}/water-heater-cost-charlotte-nc/)
            - [Emergency steps]({origin}/water-heater-emergency/)
            - [Spanish hub]({origin}/es/calentadores-de-agua-charlotte-nc/)
            - [Home]({origin}/)

            Text photos: 650-669-6890. Call: 704-839-6126.
            """;
    }

    internal static string Canonicalize(string path)
    {
        path = Uri.UnescapeDataString(path).Trim();
        if (string.IsNullOrEmpty(path)) return "/";
        if (!path.StartsWith('/')) path = "/" + path;
        path = path.ToLowerInvariant();

        var trimmed = path.TrimEnd('/');
        if (trimmed.Length == 0) return "/";

        return trimmed switch
        {
            "/home" or "/home/index" => "/",
            "/home/about" or "/home/nosotros" => "/about",
            "/home/privacidad" or "/home/privacy" => "/privacy",
            "/home/terms" => "/terms",
            "/home/problems" => "/water-heater-problems",
            "/home/faqs" => "/faqs",
            "/home/financing" => "/financing",
            "/home/safeplan" => "/financing/safe-plan",
            "/home/warranty" => "/warranty",
            "/home/register" => "/warranty/register",
            "/home/shelby" => "/shelby",
            "/home/chinagrove" or "/home/china-grove" => "/china-grove",
            _ => path
        };
    }

    private string Home(string origin) =>
        $"""
        # SAFE Water Heater — Charlotte, NC water heater replacement

        SAFE Water Heater is the water-heater specialty brand of HOME INDOR TECHNOLOGY INC. Charlotte-based crews replace tanks with published installed pricing, not a teaser tank-only number.

        ## Published installed pricing
        - 40-gal electric: $200 promotional unit + $999 standard install = **$1,199 + tax**
        - 50-gal electric: **$1,349 + tax**
        - 40-gal gas: **from $1,449 + tax** (venting and gas line may add)
        - Expansion tank from $149. Extra piping from $199. Attic, code, leak damage and relocation are written extras.

        ## How to reach SAFE
        - Text photos: 650-669-6890
        - Call: 704-839-6126
        - Quote form: {origin}/water-heater-replacement-quote/
        - Agent index: {origin}/llms.txt

        Warranty: 5-year manufacturer (model booklet) + 2-year SAFE labor. We do not promise 24/7 coverage.

        ## Next pages
        - [Charlotte replacement]({origin}/water-heater-replacement-charlotte-nc/)
        - [Installed cost]({origin}/water-heater-cost-charlotte-nc/)
        - [Topic map]({origin}/site-map/)
        - [About]({origin}/about/)
        - [Español]({origin}/es/calentadores-de-agua-charlotte-nc/)
        """;

    private static string About(string origin) =>
        $"""
        # About SAFE Water Heater

        SAFE Water Heater is the water-heater specialty brand of HOME INDOR TECHNOLOGY INC, also known as Safe Project Solution, a Charlotte-based contractor. We sell, replace, register warranties and follow up on tanks.

        - Like-for-like electric and gas tank replacement with old-unit removal when access allows
        - Photo quotes before a truck rolls
        - 5-year limited manufacturer warranty (model booklet) plus 2-year SAFE labor
        - Spanish-language coordination on the Charlotte hub pages

        Verify: call 704-839-6126, text +1 (650) 669-6890, email safeprojectsolution@gmail.com. Privacy: {origin}/privacy/ Terms: {origin}/terms/. Keep Google Business Profile in the legal name HOME INDOR TECHNOLOGY INC.

        [Topic map]({origin}/site-map/) · [llms.txt]({origin}/llms.txt)
        """;

    private static string Privacy(string origin) =>
        $"""
        # Privacy Policy — SAFE Water Heater / Safe Project Solution

        HOME INDOR TECHNOLOGY INC d/b/a SAFE Water Heater, also known as Safe Project Solution.

        We do not sell, rent, or share mobile phone numbers or SMS opt-in information with third parties or affiliates for their marketing.

        Opt-in: text +1 (650) 669-6890 after seeing that number on this website, or the optional unchecked SMS checkbox on the quote form. Message frequency varies. Message and data rates may apply. Reply STOP to unsubscribe. Reply HELP for help.

        [HTML]({origin}/privacy/) · [Terms]({origin}/terms/)
        """;

    private static string Terms(string origin) =>
        $"""
        # SMS Terms — SAFE Water Heater / Safe Project Solution

        By texting +1 (650) 669-6890 you agree to customer-care SMS from HOME INDOR TECHNOLOGY INC d/b/a SAFE Water Heater (Safe Project Solution) about water heater service in Charlotte, NC.

        Message frequency varies. Message and data rates may apply. Reply STOP to cancel, HELP for help. Mobile numbers are not sold or shared. [Privacy]({origin}/privacy/)

        [HTML]({origin}/terms/)
        """;

    private string TopicIndex(string origin)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Water heater topics for Charlotte, NC");
        sb.AppendLine();
        sb.AppendLine("Curated SAFE Water Heater guides. Related searches are grouped here instead of one URL per query.");
        sb.AppendLine();
        foreach (var group in SeoSchema.TopicGroups)
        {
            sb.AppendLine($"## {group.En}");
            foreach (var slug in group.Slugs)
            {
                if (!_catalog.TryGet(slug, out var page)) continue;
                sb.AppendLine($"- [{page.H1}]({origin}{page.Slug})");
            }

            sb.AppendLine();
        }

        sb.AppendLine($"- [XML sitemap]({origin}/sitemap.xml)");
        sb.AppendLine($"- [llms.txt]({origin}/llms.txt)");
        return sb.ToString();
    }

    private static string Problems(string origin) =>
        $"""
        # Common water heater problems in Charlotte, NC

        SAFE does not guess from one symptom. Identify fuel, size, age and safety risks before repair vs replace.

        Immediate safety: gas odor or CO alarm — leave and call 911 or the utility. Water on electrical parts — do not touch. Continuous T&P discharge — never cap the pipe.

        - [No hot water]({origin}/no-hot-water/)
        - [Leaking]({origin}/water-heater-leaking/)
        - [Emergency]({origin}/water-heater-emergency/)
        - [Repair or replace]({origin}/repair-or-replace-water-heater/)
        """;

    private static string Faqs(string origin)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Water heater FAQs — Charlotte NC (how people ask Google and chat tools)");
        sb.AppendLine();
        sb.AppendLine("Answer first. SAFE Water Heater is HOME INDOR TECHNOLOGY INC. Call 704-839-6126. Text 650-669-6890. Not 24/7.");
        sb.AppendLine();
        foreach (var group in AiSearchFaqCatalog.Groups)
        {
            sb.AppendLine($"## {group.Label}");
            sb.AppendLine();
            foreach (var item in group.Items)
            {
                sb.AppendLine($"### {item.Question}");
                sb.AppendLine();
                sb.AppendLine(item.Answer);
                sb.AppendLine();
                sb.AppendLine($"[More]({origin}{item.MoreHref})");
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }

    private static string Utility(string origin, string title, string body, string hub) =>
        $"""
        # {title}

        {body}

        [Full guide]({origin}{hub}) · [llms.txt]({origin}/llms.txt)
        """;

    private static string City(string origin, string city, string county) =>
        $"""
        # {city}, NC water heater replacement

        SAFE Water Heater is a Charlotte-based team serving homeowners in {city} ({county}). Standard packages match Charlotte: 40-gal electric $1,199 + tax, 50-gal electric $1,349 + tax, 40-gal gas from $1,449 + tax, subject to photos and access.

        [Photo quote]({origin}/water-heater-replacement-quote/) · [Pricing]({origin}/water-heater-cost-charlotte-nc/) · [Service area]({origin}/water-heater-service-near-me/)
        """;

    private static string FromSeoPage(string origin, SeoPage page)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"# {page.H1}");
        sb.AppendLine();
        sb.AppendLine(page.AnswerFirst);
        sb.AppendLine();
        if (page.ShowSafetyBar)
        {
            sb.AppendLine("**Immediate safety:** Gas odor or CO alarm: leave and call 911 or the utility. Water on electrical parts: do not touch the unit. Continuous hot discharge: never cap the T&P pipe.");
            sb.AppendLine();
        }

        if (page.ShowPricing)
        {
            sb.AppendLine("## Published installed pricing");
            sb.AppendLine("- 40-gal electric: $1,199 + tax ($200 promotional unit + $999 standard installation)");
            sb.AppendLine("- 50-gal electric: $1,349 + tax");
            sb.AppendLine("- 40-gal gas: from $1,449 + tax");
            sb.AppendLine("- Expansion tank from $149. Extra piping from $199. We do not promise 24/7 coverage.");
            sb.AppendLine();
        }

        foreach (var block in page.Sections)
        {
            sb.AppendLine($"## {block.Heading}");
            if (!string.IsNullOrWhiteSpace(block.Body))
            {
                sb.AppendLine();
                sb.AppendLine(block.Body.Trim());
            }

            if (block.Bullets is { Count: > 0 })
            {
                sb.AppendLine();
                foreach (var bullet in block.Bullets)
                {
                    sb.AppendLine($"- {bullet}");
                }
            }

            sb.AppendLine();
        }

        if (page.Faqs.Count > 0)
        {
            sb.AppendLine("## Questions grouped on this page");
            sb.AppendLine();
            foreach (var faq in page.Faqs)
            {
                sb.AppendLine($"### {faq.Question}");
                sb.AppendLine();
                sb.AppendLine(faq.Answer);
                sb.AppendLine();
            }
        }

        sb.AppendLine("## Next step");
        sb.AppendLine();
        sb.AppendLine($"Text 650-669-6890 or call 704-839-6126. [{page.CtaLabel}]({page.CtaHref}).");
        sb.AppendLine();
        if (page.Related.Count > 0)
        {
            sb.AppendLine("## Related pages");
            foreach (var link in page.Related)
            {
                var href = link.Slug.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                    ? link.Slug
                    : origin + (link.Slug.StartsWith('/') ? link.Slug : "/" + link.Slug);
                sb.AppendLine($"- [{link.Label}]({href})");
            }

            sb.AppendLine();
        }

        sb.AppendLine($"[Topic map]({origin}/site-map/) · [llms.txt]({origin}/llms.txt)");
        return sb.ToString();
    }
}
