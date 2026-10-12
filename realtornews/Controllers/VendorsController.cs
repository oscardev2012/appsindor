using Microsoft.AspNetCore.Mvc;
using RealtorNews.Data;
using RealtorNews.Models;
using RealtorNews.Services;

namespace RealtorNews.Controllers;

[Route("vendors")]
public class VendorsController : Controller
{
    private readonly RealtorNewsStore _store;

    public VendorsController(RealtorNewsStore store)
    {
        _store = store;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Preferred Vendors of Charlotte | HVAC, Roof, Crawl Space";
        ViewData["Description"] = "Charlotte preferred vendors for HVAC, water heaters, crawl space, roofing, plumbing, and painting. Request a connection with an optional Realtor code.";
        ViewData["Crumbs"] = new List<SeoCrumb> { new("Home", "/"), new("Preferred Vendors", "/vendors") };
        ViewData["Schema"] = Seo.Node("ProfessionalService", new()
        {
            ["name"] = "Preferred Vendors of Charlotte",
            ["url"] = Seo.Absolute(Request, "/vendors"),
            ["areaServed"] = Seo.Type("City", new() { ["name"] = "Charlotte" }),
            ["serviceType"] = LocalCatalog.VendorServices,
            ["description"] = ViewData["Description"]
        });
        return View(new VendorRequestViewModel());
    }

    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public IActionResult Index(VendorRequestViewModel model)
    {
        ViewData["Title"] = "Preferred Vendors of Charlotte | HVAC, Roof, Crawl Space";
        if (string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.Contact) || string.IsNullOrWhiteSpace(model.Zip))
        {
            ModelState.AddModelError("", "Name, ZIP, and a phone or email are required.");
            return View(model);
        }

        var service = LocalCatalog.VendorServices.Contains(model.Service) ? model.Service : "Inspection & Risk Review";
        var body = $"""
            Preferred vendor request
            Service: {service}
            ZIP: {model.Zip.Trim()}
            Contact: {model.Contact.Trim()}
            Realtor code: {(string.IsNullOrWhiteSpace(model.RealtorCode) ? "(none)" : model.RealtorCode.Trim())}

            {model.Description}
            """;

        try
        {
            _store.AddQuestion(new PublicQuestion
            {
                FirstName = model.FirstName,
                Email = model.Contact.Contains('@') ? model.Contact : $"{model.Zip.Trim()}@vendor-request.local",
                Topic = "Home Services",
                Body = body.Length > 2000 ? body[..2000] : body
            });
            TempData["Saved"] = "Request received. A preferred vendor can follow up. This does not commit you to a purchase.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }
}
