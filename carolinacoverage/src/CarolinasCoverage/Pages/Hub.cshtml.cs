using CarolinasCoverage.Site;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarolinasCoverage.Pages;

public class HubModel : PageModel
{
    public HubModel(SeoService seo)
    {
        Seo = seo;
    }

    public SeoService Seo { get; }
    public bool En { get; private set; }
    public HubDef Hub { get; private set; } = null!;
    public IReadOnlyList<FaqItem> Faqs { get; private set; } = [];
    public IReadOnlyList<Crumb> Breadcrumbs { get; private set; } = [];
    public IReadOnlyList<HubDef> Related { get; private set; } = [];

    public IActionResult OnGet()
    {
        if (!SeoHubCatalog.TryGetByPath(Request.Path, out var hub, out var english))
        {
            return NotFound();
        }

        Hub = hub;
        En = english;
        Faqs = FaqCatalog.For(hub.FaqKey, En);
        Related = hub.RelatedIds
            .Select(id => SeoHubCatalog.TryGet(id, out var item) ? item : null)
            .Where(item => item is not null)
            .Cast<HubDef>()
            .ToArray();
        Breadcrumbs = BuildCrumbs();
        Seo.Apply(ViewData, Seo.Hub(En, hub, Faqs, Breadcrumbs));
        return Page();
    }

    private List<Crumb> BuildCrumbs()
    {
        var crumbs = new List<Crumb>
        {
            new(En ? "Home" : "Inicio", Lang.Home(En))
        };

        if (!string.IsNullOrEmpty(Hub.ParentId) && SeoHubCatalog.TryGet(Hub.ParentId, out var parent))
        {
            crumbs.Add(new Crumb(En ? parent.H1En : parent.H1Es, parent.Path(En)));
        }

        crumbs.Add(new Crumb(En ? Hub.H1En : Hub.H1Es, Hub.Path(En)));
        return crumbs;
    }
}
