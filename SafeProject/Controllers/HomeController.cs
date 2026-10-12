using System.Diagnostics;
using CalentonesAgua.Data;
using CalentonesAgua.Models;
using CalentonesAgua.Services;
using Microsoft.AspNetCore.Mvc;

namespace CalentonesAgua.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _config;
    private readonly SitePostStore _posts;

    public HomeController(IConfiguration config, SitePostStore posts)
    {
        _config = config;
        _posts = posts;
    }

    public IActionResult Index()
    {
        SitePost? latest = null;
        try
        {
            latest = _posts.Latest("safe");
        }
        catch
        {
            latest = null;
        }

        ViewData["LatestSitePost"] = latest is { Title.Length: > 0 } ? latest : null;
        var es = SiteLanguage.IsSpanish(HttpContext);
        ViewData["HtmlLang"] = es ? "es" : "en";
        ViewData["Title"] = es
            ? "Reemplazo de calentador de agua Charlotte NC | Desde $1,199 instalado | SAFE"
            : "Water Heater Replacement Charlotte NC | From $1,199 Installed | SAFE";
        ViewData["Description"] = es
            ? "Reemplazo e instalación de calentadores de agua en Charlotte, NC. Paquetes desde $1,199 + impuesto. Cotización por fotos. Texto 650-669-6890. Llamar 704-839-6126."
            : "Water heater replacement and installation in Charlotte, NC. Hot water heater packages from $1,199 + tax. Photo quotes, Shelby and China Grove routes. Text 650-669-6890. Call 704-839-6126. Not 24/7 dispatch.";
        ViewData["CanonicalPath"] = "/";
        ViewData["HreflangAlt"] = "/es/calentadores-de-agua-charlotte-nc/";
        ViewData["HreflangAltLang"] = "es";
        ViewData["JsonLd"] = SeoSchema.Graph(
            SeoPublicOrigin.Resolve(_config, Request),
            extraTitle: "Water Heater Replacement Charlotte NC",
            pageUrl: "/",
            extraDescription: ViewData["Description"] as string);
        return View();
    }

    public IActionResult Problems()
    {
        ViewData["Title"] = "Common Water Heater Problems & Repairs in Charlotte, NC | SAFE Water Heater";
        ViewData["Description"] = "Common water heater problems in Charlotte, NC — leaking tanks, no hot water, sediment noise, rusty water and when to replace. Clustered guides, not one URL per symptom.";
        ViewData["CanonicalPath"] = "/water-heater-problems/";
        return View();
    }

    public IActionResult Faqs()
    {
        ViewData["Title"] = "Water Heater FAQs Charlotte NC | How much, leaking, near me | SAFE";
        ViewData["Description"] = "Direct answers to how people ask Google and chat tools: water heater replacement cost in Charlotte, leaking tanks, 40 vs 50 gallon, near me, financing. $1,199 + tax 40-gal electric. Call 704-839-6126.";
        ViewData["CanonicalPath"] = "/faqs/";
        ViewData["JsonLd"] = SeoSchema.FaqHub(
            SeoPublicOrigin.Resolve(_config, Request),
            AiSearchFaqCatalog.AllFaqs);
        return View(AiSearchFaqCatalog.Groups);
    }

    public IActionResult Financing()
    {
        ViewData["Title"] = "Apply for Water Heater Financing — Charlotte, NC | SAFE";
        ViewData["Description"] = "Apply for SAFE Homeowner Plan eligibility or open the Synchrony application for HOME INDOR TECHNOLOGY INC. Guide: water heater financing options.";
        ViewData["CanonicalPath"] = "/water-heater-financing/";
        return View();
    }

    public IActionResult SafePlan()
    {
        ViewData["Title"] = "Check SAFE Financing Eligibility — Charlotte, NC | SAFE Water Heater";
        ViewData["Description"] = "Start with $200 toward a qualifying promotional water heater and request financing for an eligible standard installation balance.";
        ViewData["CanonicalPath"] = "/financing/safe-plan";
        ViewData["Robots"] = "noindex";
        return View(new SafePlanViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SafePlan(SafePlanViewModel model)
    {
        ViewData["Title"] = "Check SAFE Financing Eligibility — Charlotte, NC | SAFE Water Heater";
        ViewData["CanonicalPath"] = "/financing/safe-plan";
        ViewData["Robots"] = "noindex";
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        ViewBag.Submitted = true;
        ViewBag.Reference = $"SAFE-{DateTime.Now:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
        return View(model);
    }

    public IActionResult Warranty()
    {
        ViewData["Title"] = "Register Water Heater Warranty Coverage | SAFE Water Heater";
        ViewData["Description"] = "Register the exact water heater, installation date, property and equipment photos so SAFE can verify coverage.";
        ViewData["CanonicalPath"] = "/water-heater-insurance-warranty/";
        return View();
    }

    public IActionResult Register()
    {
        ViewData["Title"] = "Register Your Water Heater Warranty Online | SAFE Water Heater";
        ViewData["Description"] = "Submit the property, equipment and installation details. You will receive a unique SAFE warranty reference when the record is saved.";
        ViewData["CanonicalPath"] = "/warranty/register";
        ViewData["Robots"] = "noindex";
        return View(new WarrantyRegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(WarrantyRegisterViewModel model)
    {
        ViewData["Title"] = "Register Your Water Heater Warranty Online | SAFE Water Heater";
        ViewData["CanonicalPath"] = "/warranty/register";
        ViewData["Robots"] = "noindex";
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        ViewBag.Submitted = true;
        ViewBag.Reference = $"WR-{DateTime.Now:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
        return View(model);
    }

    public IActionResult Shelby()
    {
        ViewData["Title"] = "Water Heater Replacement in Shelby, NC | SAFE Water Heater";
        ViewData["Description"] = "Shelby, NC water heater replacement by Charlotte-based SAFE. 40-gal electric package $1,199 + tax, old-tank removal, 5-year manufacturer + 2-year labor warranty.";
        ViewData["CanonicalPath"] = "/shelby";
        ViewData["City"] = "Shelby";
        ViewData["County"] = "Cleveland County";
        return View("City");
    }

    public IActionResult ChinaGrove()
    {
        ViewData["Title"] = "Water Heater Replacement in China Grove, NC | SAFE Water Heater";
        ViewData["Description"] = "China Grove, NC water heater replacement by SAFE. Published installed pricing from $1,199 + tax and Charlotte-based crews serving Rowan County.";
        ViewData["CanonicalPath"] = "/china-grove";
        ViewData["City"] = "China Grove";
        ViewData["County"] = "Rowan County";
        return View("City");
    }

    public IActionResult Reviews()
    {
        var es = SiteLanguage.IsSpanish(HttpContext);
        ViewData["HtmlLang"] = es ? "es" : "en";
        ViewData["Title"] = es
            ? "Deje una reseña en Google | SAFE Water Heater Charlotte NC"
            : "Leave a Google Review | SAFE Water Heater Charlotte NC";
        ViewData["Description"] = es
            ? "Después de una instalación SAFE en Charlotte, deje una reseña honesta en Google para HOME INDOR TECHNOLOGY INC. No compramos ni inventamos reseñas."
            : "After a SAFE water heater installation in Charlotte, leave an honest Google review for HOME INDOR TECHNOLOGY INC. Same-day SMS link for homeowners. We do not buy or invent reviews.";
        ViewData["CanonicalPath"] = "/reviews/";
        ViewData["JsonLd"] = SeoSchema.Graph(
            $"{Request.Scheme}://{Request.Host}",
            extraTitle: "Leave a Google Review | SAFE Water Heater",
            pageUrl: "/reviews/",
            extraDescription: ViewData["Description"] as string);
        return View();
    }

    public IActionResult Privacy()
    {
        ViewData["Title"] = "Privacy Policy | SAFE Water Heater";
        ViewData["Description"] = "HOME INDOR TECHNOLOGY INC d/b/a SAFE Water Heater and Safe Project Solution privacy policy, including SMS. We do not sell or share mobile numbers.";
        ViewData["CanonicalPath"] = "/privacy/";
        return View();
    }

    public IActionResult Terms()
    {
        ViewData["Title"] = "SMS Terms and Conditions | SAFE Water Heater";
        ViewData["Description"] = "Terms for SAFE Water Heater customer-care SMS at +1 (650) 669-6890. Message frequency varies. Message and data rates may apply. Reply STOP to cancel.";
        ViewData["CanonicalPath"] = "/terms/";
        return View();
    }

    public IActionResult About()
    {
        ViewData["Title"] = "About SAFE Water Heater | Charlotte, NC Specialists";
        ViewData["Description"] = "SAFE Water Heater is the water-heater specialty brand of HOME INDOR TECHNOLOGY INC, also known as Safe Project Solution. Charlotte-based replacement, published pricing, Spanish-language help, and warranty registration.";
        ViewData["CanonicalPath"] = "/about/";
        ViewData["JsonLd"] = SeoSchema.Graph(
            $"{Request.Scheme}://{Request.Host}",
            extraTitle: "About SAFE Water Heater",
            pageUrl: "/about/",
            extraDescription: ViewData["Description"] as string);
        return View();
    }

    public IActionResult NotFoundPage()
    {
        Response.StatusCode = 404;
        ViewData["Title"] = "Page not found | SAFE Water Heater";
        ViewData["Description"] = "That URL is not a SAFE Water Heater page. Use the Charlotte replacement, pricing, emergency or Spanish guides instead.";
        ViewData["CanonicalPath"] = null;
        ViewData["Robots"] = "noindex";
        return View("NotFound");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
