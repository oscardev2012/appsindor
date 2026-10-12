using Microsoft.Extensions.Options;

namespace MajesticRoofing.Seo;

public sealed class PageSeo
{
    public required string Lang { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Canonical { get; init; }
    public required string AlternateEn { get; init; }
    public required string AlternateEs { get; init; }
    public required string OgImage { get; init; }
    public required string OgImagePath { get; init; }
    public required string OgType { get; init; }
    public required string OgLocale { get; init; }
    public required string OgLocaleAlternate { get; init; }
    public required string Robots { get; init; }
    public required string Origin { get; init; }
    public required string Instagram { get; init; }
    public required string Facebook { get; init; }
    public string? JsonLd { get; init; }
    public string? GoogleSiteVerification { get; init; }
    public string? BingSiteVerification { get; init; }
    public bool Indexable { get; init; } = true;
}

public interface ISeoService
{
    string Origin { get; }
    PageSeo Current { get; }
    string SitemapXml();
    string RobotsTxt();
    string LlmsTxt();
}

public sealed class SeoService : ISeoService
{
    private readonly SeoCatalog _catalog;
    private readonly FaqHub _faqs;
    private readonly SeoOptions _options;
    private readonly IHttpContextAccessor _http;

    public SeoService(SeoCatalog catalog, FaqHub faqs, IOptions<SeoOptions> options, IHttpContextAccessor http)
    {
        _catalog = catalog;
        _faqs = faqs;
        _options = options.Value;
        _http = http;
    }

    public string Origin
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(_options.SiteUrl))
                return _options.SiteUrl.TrimEnd('/');

            var req = _http.HttpContext?.Request;
            if (req is null) return "https://roofingmajesticcharlotte.com";
            return $"{req.Scheme}://{req.Host.Value}".TrimEnd('/');
        }
    }

    public PageSeo Current
    {
        get
        {
            var ctx = _http.HttpContext;
            var path = SeoCatalog.Normalize(ctx?.Request.Path.Value);
            var route = _catalog.Find(path);
            var isEs = _catalog.IsSpanish(path);
            var isError = path.Equals("/Error", StringComparison.OrdinalIgnoreCase);

            var title = route is null
                ? (isEs
                    ? "Majestic Roofing | Charlotte y Rock Hill"
                    : "Majestic Roofing | Charlotte & Rock Hill")
                : (isEs ? route.EsTitle : route.EnTitle);
            var description = route is null
                ? (isEs
                    ? "Roofing bilingüe, reclamos de techo, reparaciones y reemplazo en Charlotte, NC y Rock Hill, SC."
                    : "Bilingual roofing, insurance-claim documentation, repairs and replacement in Charlotte, NC and Rock Hill, SC.")
                : (isEs ? route.EsDescription : route.EnDescription);

            if (isError)
            {
                var code = ctx?.Request.Query["code"].ToString();
                var notFound = code == "404" || ctx?.Response.StatusCode == 404;
                title = notFound ? "Page not found | Majestic Roofing" : "Error | Majestic Roofing";
                description = "The requested Majestic Roofing page could not be shown.";
            }

            var imagePath = route?.OgImage ?? _options.DefaultImage;
            var ogImage = imagePath.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                ? imagePath
                : Origin + imagePath;
            var canonical = Origin + (path == "/" ? "/" : path);

            return new PageSeo
            {
                Lang = isEs ? "es" : "en",
                Title = title,
                Description = description,
                Canonical = canonical,
                AlternateEn = Origin + (route?.EnPath ?? "/"),
                AlternateEs = Origin + (route?.EsPath ?? "/es"),
                OgImage = ogImage,
                OgImagePath = imagePath.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? imagePath : imagePath,
                OgType = route?.OgType ?? "website",
                OgLocale = isEs ? "es_US" : "en_US",
                OgLocaleAlternate = isEs ? "en_US" : "es_US",
                Robots = isError ? "noindex, nofollow" : "index, follow, max-image-preview:large, max-snippet:-1, max-video-preview:-1",
                Origin = Origin,
                Instagram = _options.Instagram,
                Facebook = _options.Facebook,
                Indexable = !isError,
                GoogleSiteVerification = string.IsNullOrWhiteSpace(_options.GoogleSiteVerification) ? null : _options.GoogleSiteVerification,
                BingSiteVerification = string.IsNullOrWhiteSpace(_options.BingSiteVerification) ? null : _options.BingSiteVerification,
                JsonLd = isError ? null : BuildJsonLd(route, isEs, title, description, canonical, ogImage)
            };
        }
    }

    public string SitemapXml()
    {
        var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("""<?xml version="1.0" encoding="UTF-8"?>""");
        sb.AppendLine("""<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9" xmlns:xhtml="http://www.w3.org/1999/xhtml" xmlns:image="http://www.google.com/schemas/sitemap-image/1.1">""");
        foreach (var route in _catalog.Routes)
        {
            WriteUrl(sb, Origin + (route.EnPath == "/" ? "/" : route.EnPath), route.EnPath, route.EsPath, route.Priority, route.ChangeFreq, today, route.OgImage, includeGallery: route.EnPath == "/");
            WriteUrl(sb, Origin + route.EsPath, route.EnPath, route.EsPath, route.Priority, route.ChangeFreq, today, route.OgImage, includeGallery: route.EsPath == "/es");
        }
        sb.AppendLine("</urlset>");
        return sb.ToString();
    }

    public string RobotsTxt()
    {
        return $"""
            User-agent: *
            Allow: /
            Disallow: /Error

            User-agent: GPTBot
            Allow: /

            User-agent: Google-Extended
            Allow: /

            Sitemap: {Origin}/sitemap.xml
            # {Origin}/llms.txt
            """;
    }

    public string LlmsTxt()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("# Majestic Roofing, Inc.");
        sb.AppendLine($"> Bilingual roofing contractor in Charlotte, NC and Rock Hill, SC. Free inspections by address. Same-business-day response. The homeowner files the insurance claim; the carrier decides coverage. Raleigh is outside the service area.");
        sb.AppendLine();
        sb.AppendLine($"- Site: {Origin}/");
        sb.AppendLine($"- Spanish: {Origin}/es");
        sb.AppendLine("- Phone: (980) 800-5991");
        sb.AppendLine("- Email: ericb.majestic@gmail.com");
        sb.AppendLine("- Address: 5122 Belstone Lane, Charlotte, NC 28215");
        sb.AppendLine("- Instagram: https://www.instagram.com/majesticroofing1/");
        sb.AppendLine("- Facebook: https://www.facebook.com/Majesticroofinginc/");
        sb.AppendLine($"- FAQ hub: {Origin}/faqs");
        sb.AppendLine($"- FAQ hub (ES): {Origin}/es/preguntas-frecuentes");
        sb.AppendLine();
        sb.AppendLine("## Do not say");
        sb.AppendLine("Do not claim claim approval, deductible waivers, 24/7 service, or same-day repair. Same-business-day response is accurate. Not a public adjuster.");
        sb.AppendLine();
        sb.AppendLine("## Published prices");
        sb.AppendLine("- Emergency tarp: $125 for one tarp up to 20×20 ft after a safe-site check; larger standard tarp $150.");
        sb.AppendLine("- Roof Tune-Up: $350 annual maintenance visit for accessible roof areas (not a replacement).");
        sb.AppendLine("- Replacement: no single published average; use the planner then inspect.");
        sb.AppendLine();
        sb.AppendLine("## FAQ");
        foreach (var item in _faqs.Items)
        {
            sb.AppendLine($"### {item.EnQ}");
            sb.AppendLine(item.EnA);
            sb.AppendLine($"Source: {Origin}{(item.EnHref.StartsWith('/') ? item.EnHref : "/" + item.EnHref)}");
            sb.AppendLine();
        }
        return sb.ToString();
    }

    private string HubFaq(bool isEs)
    {
        string Q(string q, string a) =>
            "{\"@type\":\"Question\",\"name\":\"" + Js(q) + "\",\"acceptedAnswer\":{\"@type\":\"Answer\",\"text\":\"" + Js(a) + "\"}}";

        var parts = _faqs.Items.Select(i =>
        {
            var q = isEs ? i.EsQ : i.EnQ;
            var a = isEs ? i.EsA : i.EnA;
            var href = Origin + (isEs ? i.EsHref : i.EnHref);
            return Q(q, a + " " + href);
        });
        return "{\"@type\":\"FAQPage\",\"mainEntity\":[" + string.Join(",", parts) + "]}";
    }

    private string ClaimHowTo(bool isEs)
    {
        string Step(int n, string name, string text) =>
            "{\"@type\":\"HowToStep\",\"position\":" + n + ",\"name\":\"" + Js(name) + "\",\"text\":\"" + Js(text) + "\"}";

        if (isEs)
        {
            return "{\"@type\":\"HowTo\",\"name\":\"Qué hacer después de daño de tormenta en el techo\",\"description\":\"El dueño presenta el claim. Majestic documenta el techo. La aseguradora decide cobertura.\",\"step\":[" +
                Step(1, "Proteger personas y el interior", "No camine un techo mojado. Recoja agua adentro si puede. Llame al 911 si hay peligro inmediato.") + "," +
                Step(2, "Documentar desde el suelo", "Fotografíe manchas, shingles visibles y escombros cuando sea seguro. Guarde recibos.") + "," +
                Step(3, "Abrir el claim con su aseguradora", "Usted presenta el reclamo. Pida el número de claim y una copia del estimate cuando exista.") + "," +
                Step(4, "Inspección de contratista", "Majestic puede inspeccionar condiciones visibles, preparar un alcance de construcción y reunirse con el adjuster si usted lo autoriza.") + "," +
                Step(5, "Comparar alcance vs estimate", "Un suplemento es conversación de construcción, no una promesa de más dinero. La compañía decide pago.") +
                "]}";
        }

        return "{\"@type\":\"HowTo\",\"name\":\"What to do after storm damage to a roof\",\"description\":\"The homeowner files the claim. Majestic documents the roof. The carrier decides coverage.\",\"step\":[" +
            Step(1, "Protect people and the interior", "Do not walk a wet roof. Catch water inside if you can. Call 911 if anyone is in immediate danger.") + "," +
            Step(2, "Document from the ground", "Photograph stains, visible shingles and debris when it is safe. Keep receipts.") + "," +
            Step(3, "Open the claim with your carrier", "You file the claim. Ask for the claim number and a copy of the estimate when it exists.") + "," +
            Step(4, "Contractor inspection", "Majestic can inspect visible conditions, prepare a construction scope and meet the adjuster when you authorize it.") + "," +
            Step(5, "Compare scope to the estimate", "A supplement is a construction conversation, not a promise of more money. The carrier decides payment.") +
            "]}";
    }

    private string? PageFaq(string enPath, bool isEs)
    {
        string Q(string q, string a) =>
            "{\"@type\":\"Question\",\"name\":\"" + Js(q) + "\",\"acceptedAnswer\":{\"@type\":\"Answer\",\"text\":\"" + Js(a) + "\"}}";

        var items = enPath switch
        {
            "/repair-or-replace-roof" => isEs
                ? new[]
                {
                    Q("¿Se puede saber desde la calle?", "No. Pitch, decking, flashing y manchas interiores hay que inspeccionarlos. Una foto desde el bordillo es un inicio, no un alcance."),
                    Q("Si la aseguranza paga, ¿tengo que reemplazar?", "La cobertura es decisión de la compañía. Lo que necesita el techo es pregunta de construcción. Pueden diferir. Majestic escribe el alcance de construcción; no condonamos deductibles ni garantizamos pago."),
                    Q("¿Puedo reemplazar solo un slope?", "A veces. El color y la vida restante igual importan. Explicamos cuándo un reemplazo parcial es honesto y cuándo crea una línea de liqueo después."),
                    Q("¿Una tormenta significa techo nuevo?", "No automáticamente. Los patrones visibles se documentan como hechos. La aseguradora decide cobertura. La construcción igual tiene que coincidir con el techo que hay."),
                    Q("¿El Tune-Up evita un reemplazo?", "El Tune-Up es mantenimiento en zonas accesibles. No repara liqueos activos ni reemplaza un techo al final de su vida.")
                }
                : new[]
                {
                    Q("Can you tell from the street?", "No. Pitch, decking, flashing and interior stains have to be inspected. A photo from the curb is a start, not a scope."),
                    Q("If insurance pays, do I have to replace?", "Coverage is the carrier’s decision. What the roof needs is a construction question. Those can differ. Majestic writes the construction scope; we do not waive deductibles or guarantee payment."),
                    Q("Can I replace only one slope?", "Sometimes. Matching color and remaining life still matter. We explain when a partial replacement is honest and when it creates a future leak line."),
                    Q("Does a storm automatically mean a new roof?", "No. Visible storm patterns are documented as facts. The carrier decides coverage. Construction still has to match the roof that is actually there."),
                    Q("Will a Tune-Up avoid replacement?", "The Tune-Up is maintenance on accessible areas. It is not a leak repair and not a replacement for a roof at the end of its life.")
                },
            "/roof-tune-up-charlotte-nc" => isEs
                ? new[]
                {
                    Q("¿El Tune-Up es un reemplazo de techo?", "No. Es mantenimiento en zonas accesibles. Un reemplazo es otro proyecto con alcance escrito de construcción."),
                    Q("¿Incluye reparaciones interiores?", "No. Interiores, decking, estructural y condiciones ocultas requieren un acuerdo separado."),
                    Q("¿Hacen Tune-Up en Raleigh?", "No. La disponibilidad se confirma por dirección a unos 90 minutos de Charlotte, NC o Rock Hill, SC."),
                    Q("¿Sirve el Tune-Up para un liqueo activo?", "No. Un liqueo activo, agujero o daño de tormenta amplio necesita reparación, lona o inspección, no solo mantenimiento."),
                    Q("¿Cada cuánto se recomienda?", "Está pensado como visita anual de mantenimiento general, no como respuesta a una tormenta.")
                }
                : new[]
                {
                    Q("Is the Tune-Up a full roof replacement?", "No. It is maintenance on accessible areas. A replacement is a different project with a written construction scope."),
                    Q("Does the Tune-Up include interior repairs?", "No. Interior work, decking, structural items and hidden conditions require a separate agreement."),
                    Q("Do you serve Raleigh for Tune-Ups?", "No. Availability is confirmed by address within about 90 minutes of Charlotte, NC or Rock Hill, SC."),
                    Q("Will a Tune-Up stop an active leak?", "No. An active leak, open hole or widespread storm damage needs repair, tarping or inspection—not maintenance alone."),
                    Q("How often is a Tune-Up recommended?", "It is planned as a general annual maintenance visit, not as a storm-response service.")
                },
            _ => null
        };

        return items is null ? null : "{\"@type\":\"FAQPage\",\"mainEntity\":[" + string.Join(",", items) + "]}";
    }

    private void WriteUrl(System.Text.StringBuilder sb, string loc, string enPath, string esPath, double priority, string freq, string lastmod, string image, bool includeGallery)
    {
        sb.AppendLine("  <url>");
        sb.AppendLine($"    <loc>{Xml(loc)}</loc>");
        sb.AppendLine($"    <lastmod>{lastmod}</lastmod>");
        sb.AppendLine($"    <changefreq>{freq}</changefreq>");
        sb.AppendLine($"    <priority>{priority.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture)}</priority>");
        sb.AppendLine($"    <xhtml:link rel=\"alternate\" hreflang=\"en\" href=\"{Xml(Origin + (enPath == "/" ? "/" : enPath))}\" />");
        sb.AppendLine($"    <xhtml:link rel=\"alternate\" hreflang=\"es\" href=\"{Xml(Origin + esPath)}\" />");
        sb.AppendLine($"    <xhtml:link rel=\"alternate\" hreflang=\"x-default\" href=\"{Xml(Origin + (enPath == "/" ? "/" : enPath))}\" />");
        sb.AppendLine($"    <image:image><image:loc>{Xml(Origin + image)}</image:loc></image:image>");
        if (includeGallery)
        {
            for (var i = 1; i <= 16; i++)
            {
                sb.AppendLine($"    <image:image><image:loc>{Xml(Origin + $"/projects/roof-project-{i:00}.jpeg")}</image:loc><image:title>Majestic Roofing project {i:00}</image:title></image:image>");
            }
        }
        sb.AppendLine("  </url>");
    }

    private string BuildJsonLd(SeoRoute? route, bool isEs, string title, string description, string canonical, string ogImage)
    {
        var o = _options;
        var businessId = Origin + "/#business";
        var websiteId = Origin + "/#website";
        var homeUrl = isEs ? Origin + "/es" : Origin + "/";
        var slogan = isEs
            ? "Evidencia del techo. Alcance claro. Roofing profesional."
            : "Roof evidence. Clear scope. Professional roofing.";
        var lat = o.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
        var lng = o.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
        var map = "https://www.google.com/maps/search/?api=1&query=" + Uri.EscapeDataString($"{o.StreetAddress}, {o.Locality}, {o.Region} {o.PostalCode}");
        var area = "[\"Charlotte, NC\",\"Matthews, NC\",\"Mint Hill, NC\",\"Huntersville, NC\",\"Concord, NC\",\"Gastonia, NC\",\"Monroe, NC\",\"Rock Hill, SC\",\"Fort Mill, SC\",\"Tega Cay, SC\",\"Indian Land, SC\",\"Lake Wylie, SC\"]";
        var offers = "[{\"@type\":\"Offer\",\"itemOffered\":{\"@type\":\"Service\",\"name\":\"Roof replacement\"}},{\"@type\":\"Offer\",\"itemOffered\":{\"@type\":\"Service\",\"name\":\"Roof repair and leak diagnosis\"}},{\"@type\":\"Offer\",\"itemOffered\":{\"@type\":\"Service\",\"name\":\"Storm damage roof inspection and documentation\"}},{\"@type\":\"Offer\",\"priceCurrency\":\"USD\",\"price\":\"125\",\"itemOffered\":{\"@type\":\"Service\",\"name\":\"Emergency roof tarping\"}},{\"@type\":\"Offer\",\"itemOffered\":{\"@type\":\"Service\",\"name\":\"Gutters, fascia and soffit\"}},{\"@type\":\"Offer\",\"priceCurrency\":\"USD\",\"price\":\"350\",\"itemOffered\":{\"@type\":\"Service\",\"name\":\"Roof Tune-Up maintenance\"}}]";
        var sameAs = "[\"" + Js(o.Instagram) + "\",\"" + Js(o.Facebook) + "\",\"" + Js(o.BbbUrl) + "\"]";
        var business = "{\"@type\":[\"HomeAndConstructionBusiness\",\"RoofingContractor\"],\"@id\":\"" + businessId + "\",\"name\":\"" + Js(o.SiteName) + "\",\"legalName\":\"" + Js(o.SiteName) + "\",\"url\":\"" + Origin + "/\",\"image\":\"" + ogImage + "\",\"logo\":{\"@type\":\"ImageObject\",\"url\":\"" + Origin + "/favicon.svg\"},\"telephone\":\"" + o.Phone + "\",\"email\":\"" + o.Email + "\",\"foundingDate\":\"2021-06-16\",\"priceRange\":\"$$\",\"currenciesAccepted\":\"USD\",\"hasMap\":\"" + map + "\",\"slogan\":\"" + Js(slogan) + "\",\"description\":\"" + Js(description) + "\",\"address\":{\"@type\":\"PostalAddress\",\"streetAddress\":\"" + Js(o.StreetAddress) + "\",\"addressLocality\":\"" + o.Locality + "\",\"addressRegion\":\"" + o.Region + "\",\"postalCode\":\"" + o.PostalCode + "\",\"addressCountry\":\"" + o.Country + "\"},\"geo\":{\"@type\":\"GeoCoordinates\",\"latitude\":" + lat + ",\"longitude\":" + lng + "},\"areaServed\":" + area + ",\"availableLanguage\":[\"English\",\"Spanish\"],\"sameAs\":" + sameAs + ",\"contactPoint\":[{\"@type\":\"ContactPoint\",\"telephone\":\"" + o.Phone + "\",\"email\":\"" + o.Email + "\",\"contactType\":\"customer service\",\"availableLanguage\":[\"English\",\"Spanish\"],\"areaServed\":[\"US\"]}],\"hasOfferCatalog\":{\"@type\":\"OfferCatalog\",\"name\":\"Majestic Roofing services\",\"itemListElement\":" + offers + "}}";
        var website = "{\"@type\":\"WebSite\",\"@id\":\"" + websiteId + "\",\"url\":\"" + Origin + "/\",\"name\":\"Majestic Roofing\",\"inLanguage\":[\"en-US\",\"es-US\"],\"publisher\":{\"@id\":\"" + businessId + "\"}}";
        var crumbs = "{\"@type\":\"ListItem\",\"position\":1,\"name\":\"" + (isEs ? "Inicio" : "Home") + "\",\"item\":\"" + homeUrl + "\"}";
        if (route is not null && route.EnPath != "/")
            crumbs += ",{\"@type\":\"ListItem\",\"position\":2,\"name\":\"" + Js(title) + "\",\"item\":\"" + canonical + "\"}";
        var breadcrumb = "{\"@type\":\"BreadcrumbList\",\"itemListElement\":[" + crumbs + "]}";
        var webpage = "{\"@type\":\"WebPage\",\"@id\":\"" + canonical + "#webpage\",\"url\":\"" + canonical + "\",\"name\":\"" + Js(title) + "\",\"description\":\"" + Js(description) + "\",\"inLanguage\":\"" + (isEs ? "es-US" : "en-US") + "\",\"isPartOf\":{\"@id\":\"" + websiteId + "\"},\"about\":{\"@id\":\"" + businessId + "\"},\"primaryImageOfPage\":\"" + ogImage + "\"}";
        var graph = business + "," + website + "," + webpage + "," + breadcrumb;
        if (route is null || route.EnPath == "/")
            graph += "," + HomeFaq(isEs);
        else if (route.EnPath == "/faqs")
            graph += "," + HubFaq(isEs);
        else if (route.EnPath == "/roof-claim-process")
            graph += "," + ClaimHowTo(isEs);
        else
        {
            var pageFaq = PageFaq(route.EnPath, isEs);
            if (pageFaq is not null)
                graph += "," + pageFaq;
        }
        return "{\"@context\":\"https://schema.org\",\"@graph\":[" + graph + "]}";
    }

    private static string HomeFaq(bool isEs)
    {
        string Q(string q, string a) =>
            "{\"@type\":\"Question\",\"name\":\"" + Js(q) + "\",\"acceptedAnswer\":{\"@type\":\"Answer\",\"text\":\"" + Js(a) + "\"}}";

        if (isEs)
        {
            return "{\"@type\":\"FAQPage\",\"mainEntity\":[" +
                Q("¿Qué incluye el Roof Tune-Up de $350?", "Es una visita anual de mantenimiento general para áreas accesibles del techo. El alcance planeado incluye revisión visual, mantenimiento de sellador en clavos expuestos y penetraciones accesibles, detalles de entrada de agua en chimeneas y revisión de preocupaciones visibles. No incluye reemplazo de techo, reparaciones por condiciones ocultas, trabajo estructural, plywood/decking, reparaciones mayores, reparaciones interiores ni trabajos que requieran un alcance separado.") + "," +
                Q("¿Cuál es el precio de la lona de emergencia?", "El servicio publicado comienza en $125 por una lona de hasta 20 × 20 pies, sujeto a una evaluación segura de la propiedad. Una lona estándar más grande cuesta $150. Varias lonas, cobertura de toda la casa, acceso inusual, clima inseguro, techos inclinados o materiales adicionales requieren confirmación separada antes de iniciar.") + "," +
                Q("¿Majestic puede ayudar con mi reclamo de seguro?", "Majestic puede inspeccionar condiciones visibles, documentar el proyecto de roofing, preparar un estimate o alcance y reunirse en la propiedad con un adjuster si se solicita. Majestic no decide cobertura, no garantiza aprobación de reclamo ni actúa como public adjuster a menos que esté separado y debidamente autorizado.") + "," +
                Q("¿La calculadora es una cotización final?", "No. Es un rango inicial basado en la información que proporciona. La geometría, medidas, materiales, acceso, requisitos de código, permisos, decking y condiciones ocultas se confirman en persona.") + "," +
                Q("¿Qué ciudades atienden?", "Majestic atiende propiedades aproximadamente a 90 minutos de Charlotte, NC o Rock Hill, SC. Las inspecciones de techo y relacionadas con reclamo no tienen cargo de viaje. La disponibilidad se confirma por dirección y Raleigh no forma parte del área.") +
                "]}";
        }

        return "{\"@type\":\"FAQPage\",\"mainEntity\":[" +
            Q("What is included in the $350 Roof Tune-Up?", "It is a general annual maintenance visit for accessible roof areas. The planned scope includes a visual roof review, maintenance caulking or sealant at exposed fasteners and accessible roof penetrations, chimney water-entry details, and a review for visible roof concerns. It does not include roof replacement, hidden-condition repairs, structural work, decking, major repairs, interior repairs, or items requiring a separate scope.") + "," +
            Q("What is the emergency tarp price?", "The published starting emergency tarp service is $125 for one tarp up to 20 × 20 feet, subject to a safe site assessment. A larger standard tarp is $150. Multiple tarps, whole-roof coverage, unusual access, unsafe weather, steep roofs, or additional materials require a separate confirmation before work begins.") + "," +
            Q("Can Majestic help with my insurance claim?", "Majestic can inspect visible conditions, document the roofing project, prepare a roofing estimate or scope, and meet at the property with an adjuster when requested. Majestic does not decide coverage, guarantee claim approval, or act as a public adjuster unless separately licensed and authorized.") + "," +
            Q("Is the price planner a final quote?", "No. It is an early planning range based on the information you provide. Roof geometry, measurements, materials, access, code requirements, permits, decking, and hidden conditions are confirmed in person.") + "," +
            Q("What cities do you serve?", "Majestic serves properties approximately 90 minutes from Charlotte, NC or Rock Hill, SC. Roof and claim-related inspections do not have a travel fee. Availability is confirmed by address, and Raleigh is outside the service area.") +
            "]}";
    }

    private static string Xml(string value) => System.Security.SecurityElement.Escape(value) ?? value;
    private static string Js(string value) => value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", " ");
}
