using CalentonesAgua.Data;
using CalentonesAgua.Models;
using CalentonesAgua.Services;
using Microsoft.AspNetCore.Mvc;

namespace CalentonesAgua.Controllers;

public class SeoController : Controller
{
    private readonly SeoCatalog _catalog;
    private readonly QuoteLeadStore _leads;

    public SeoController(SeoCatalog catalog, QuoteLeadStore leads)
    {
        _catalog = catalog;
        _leads = leads;
    }

    [HttpGet]
    public IActionResult Page(string slug) => Render(slug);

    [HttpGet]
    public IActionResult Es(string slug) => Render("es/" + slug);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Quote(SeoQuoteViewModel model)
    {
        BindPage("/water-heater-replacement-quote/");
        CaptureAttribution(model);

        if (!string.IsNullOrWhiteSpace(model.CompanyWebsite))
        {
            ViewBag.Submitted = true;
            ViewBag.SmsConsent = false;
            ViewBag.Reference = $"QT-{DateTime.UtcNow:yyyyMMdd}-0000";
            return View("Quote", new SeoQuoteViewModel());
        }

        if (!ModelState.IsValid)
        {
            return View("Quote", model);
        }

        ViewBag.Submitted = true;
        ViewBag.SmsConsent = model.SmsConsent;
        ViewBag.Reference = _leads.Save(model);
        // If SmsConsent is false, do not text the customer. Call 704-839-6126 or wait until they text +1 (650) 669-6890.
        return View("Quote", new SeoQuoteViewModel());
    }

    private IActionResult Render(string slug)
    {
        if (!_catalog.TryGet(slug, out var page))
        {
            return NotFound();
        }

        BindMeta(page);
        if (page.IsQuotePage)
        {
            var model = new SeoQuoteViewModel();
            CaptureAttribution(model);
            return View("Quote", model);
        }

        return View("Page", page);
    }

    private void BindPage(string slug)
    {
        if (_catalog.TryGet(slug, out var page))
        {
            BindMeta(page);
        }
    }

    private void BindMeta(SeoPage page)
    {
        ViewData["Title"] = page.Title;
        ViewData["Description"] = page.Description;
        ViewData["HtmlLang"] = page.Language.Equals("es", StringComparison.OrdinalIgnoreCase) ? "es" : "en";
        if (string.Equals(ViewData["HtmlLang"] as string, "es", StringComparison.OrdinalIgnoreCase))
        {
            SiteLanguage.SetCookie(Response, SiteLanguage.Spanish);
        }
        ViewData["CanonicalPath"] = page.Slug;
        ViewData["SeoPage"] = page;
        if (!string.IsNullOrEmpty(page.AlternateLangSlug))
        {
            ViewData["HreflangAlt"] = page.AlternateLangSlug;
            ViewData["HreflangAltLang"] = page.Language.Equals("es", StringComparison.OrdinalIgnoreCase) ? "en" : "es";
        }

        ViewData["OgType"] = "article";
        ViewData["JsonLd"] = SeoSchema.Graph($"{Request.Scheme}://{Request.Host}", page);
        if (page.IsQuotePage)
        {
            ViewData["QuoteLanding"] = true;
        }
    }

    private void CaptureAttribution(SeoQuoteViewModel model)
    {
        model.UtmSource = FirstNonEmpty(model.UtmSource, Request.Query["utm_source"]);
        model.UtmCampaign = FirstNonEmpty(model.UtmCampaign, Request.Query["utm_campaign"]);
        model.Gclid = FirstNonEmpty(model.Gclid, Request.Query["gclid"]);
        if (string.IsNullOrWhiteSpace(model.LandingPath))
        {
            model.LandingPath = Request.Path + Request.QueryString;
        }
    }

    private static string? FirstNonEmpty(string? a, string? b)
    {
        var left = (a ?? "").Trim();
        if (left.Length > 0)
        {
            return left;
        }

        var right = (b ?? "").Trim();
        return right.Length == 0 ? null : right;
    }

    [HttpGet]
    public IActionResult TopicIndex()
    {
        ViewData["Title"] = "Water Heater Topics for Charlotte, NC | SAFE Water Heater";
        ViewData["Description"] = "All SAFE Water Heater guides grouped by search intent: replacement, cost, emergencies, diagnosis, sizing, warranty and Spanish-language help for Charlotte, Shelby and China Grove.";
        ViewData["CanonicalPath"] = "/site-map/";
        var origin = $"{Request.Scheme}://{Request.Host}";
        ViewData["JsonLd"] = SeoSchema.Graph(
            origin,
            extraTitle: "Water heater topics",
            pageUrl: "/site-map/",
            extraDescription: ViewData["Description"] as string);
        var groups = SeoSchema.TopicGroups
            .Select(g => new SeoHubGroup(
                g.En,
                g.Slugs.Select(slug => _catalog.TryGet(slug, out var page) ? page : null).OfType<SeoPage>().ToList()))
            .Where(g => g.Pages.Count > 0)
            .ToList();
        return View(groups);
    }
}
