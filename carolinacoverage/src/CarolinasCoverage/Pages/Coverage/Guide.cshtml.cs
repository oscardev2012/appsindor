using CarolinasCoverage.Site;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarolinasCoverage.Pages.Coverage;

public class GuideModel : PageModel
{
    public GuideModel(SiteCatalog catalog, SeoService seo)
    {
        Catalog = catalog;
        Seo = seo;
    }

    public SiteCatalog Catalog { get; }
    public SeoService Seo { get; }
    public bool En { get; private set; }
    public string ProductId { get; private set; } = "auto";
    public GuideProduct Product { get; private set; } = null!;
    public GuideLocale Copy { get; private set; } = null!;
    public IReadOnlyList<Crumb> Breadcrumbs { get; private set; } = [];

    public IActionResult OnGet(string product)
    {
        if (!Catalog.Guides.TryGetValue(product, out var guide))
        {
            return NotFound();
        }

        En = Lang.IsEnglish(Request);
        if (SeoHubCatalog.TryProduct(product, out var hub))
        {
            return RedirectPermanent(hub.Path(En));
        }
        ProductId = product;
        Product = guide;
        Copy = En ? guide.En : guide.Es;
        Breadcrumbs =
        [
            new Crumb(En ? "Home" : "Inicio", Lang.Home(En)),
            new Crumb(En ? "Coverage" : "Coberturas", Lang.Home(En) + "#products"),
            new Crumb(En ? ToTitle(Copy.Label) : Copy.Label, Lang.Coverage(En, product))
        ];
        Seo.Apply(ViewData, Seo.Guide(En, product, guide, Copy));
        return Page();
    }

    private static string ToTitle(string label) =>
        string.Join(' ', label.ToLowerInvariant().Split(' ').Select(w =>
            w.Length == 0 ? w : char.ToUpperInvariant(w[0]) + w[1..]));
}
