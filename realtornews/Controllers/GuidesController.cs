using Microsoft.AspNetCore.Mvc;
using RealtorNews.Data;

namespace RealtorNews.Controllers;

public class GuidesController : Controller
{
    [HttpGet("/guides")]
    public IActionResult Index(string? category)
    {
        var selected = string.IsNullOrWhiteSpace(category) ? "all" : category;
        ViewBag.Category = selected;
        ViewBag.Guides = string.Equals(selected, "all", StringComparison.OrdinalIgnoreCase)
            ? LocalCatalog.StageGuides
            : LocalCatalog.StageGuides.Where(g => g.Category.Equals(selected, StringComparison.OrdinalIgnoreCase)).ToList();
        ViewData["Title"] = selected switch
        {
            "Buying" => "Charlotte home buying guides",
            "Selling" => "Charlotte home selling guides",
            "Investing" => "Charlotte rental and investing guides",
            "Homeownership" => "Charlotte homeownership and maintenance guides",
            _ => "Charlotte real estate guides"
        };
        ViewData["Description"] = selected switch
        {
            "Buying" => "What Charlotte buyers should know about cash to close, inspections, and how to read a report before changing the budget.",
            "Selling" => "What to fix before listing a Charlotte home, how Realtors build a pricing range, and how to prepare a winter showing.",
            "Investing" => "A simple way to evaluate a Charlotte-area rental: price, repairs, rent, operating costs, and exit options.",
            "Homeownership" => "First-year maintenance, seasonal checks, and home-system questions for Charlotte homeowners.",
            _ => "Plain-language Charlotte guides for buying, selling, investing, and taking care of a home."
        };
        ViewData["Crumbs"] = new List<SeoCrumb>
        {
            new("Home", "/"),
            new("Guides", "/guides"),
            new(selected == "all" ? "All guides" : selected, selected == "all" ? "/guides" : $"/guides?category={selected}")
        };
        return View();
    }

    [HttpGet("/cities")]
    public IActionResult Cities()
    {
        ViewData["Title"] = "Charlotte-area cities guide";
        ViewData["Description"] = "Compare 43 Charlotte-region communities in NC and SC: commute, home type, city limits, and questions to ask before you tour.";
        ViewData["Crumbs"] = new List<SeoCrumb> { new("Home", "/"), new("Cities", "/cities") };
        return View();
    }

    [HttpGet("/seasonal")]
    public IActionResult Seasonal()
    {
        ViewData["Title"] = "Charlotte seasonal home guide";
        ViewData["Description"] = "Fall and winter home checks for Charlotte: heating, roof, gutters, crawl space, drainage, and storm follow-up from Realtors and preferred vendors.";
        ViewData["Crumbs"] = new List<SeoCrumb> { new("Home", "/"), new("Seasonal Guide", "/seasonal") };
        return View();
    }
}
