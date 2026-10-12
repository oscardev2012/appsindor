using System.Text.Json;
using CarolinasCoverage.Site;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarolinasCoverage.Pages.Service;

public class DemoModel : PageModel
{
    public DemoModel(SiteCatalog catalog, SeoService seo)
    {
        Catalog = catalog;
        Seo = seo;
    }

    public SiteCatalog Catalog { get; }
    public SeoService Seo { get; }
    public bool En { get; private set; }
    public string Type { get; private set; } = "change";
    public ServiceLocale Copy { get; private set; } = null!;
    public string Payload { get; private set; } = "{}";

    public IActionResult OnGet(string type)
    {
        if (!Catalog.Services.TryGetValue(type, out var flow))
        {
            return NotFound();
        }

        En = Lang.IsEnglish(Request);
        Type = type;
        Copy = En ? flow.En : flow.Es;
        Seo.Apply(ViewData, Seo.Service(En, Copy.Title, Copy.Intro));
        Payload = JsonSerializer.Serialize(new
        {
            type,
            lang = En ? "en" : "es",
            home = Lang.Home(En),
            esUrl = Lang.ToSpanish(Request),
            enUrl = Lang.ToEnglish(Request),
            copy = Copy
        }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        return Page();
    }
}
