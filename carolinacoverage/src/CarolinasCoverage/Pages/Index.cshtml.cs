using CarolinasCoverage.Site;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarolinasCoverage.Pages;

public class IndexModel : PageModel
{
    public IndexModel(SiteCatalog catalog, SeoService seo)
    {
        Catalog = catalog;
        Seo = seo;
    }

    public SiteCatalog Catalog { get; }
    public SeoService Seo { get; }
    public bool En { get; private set; }
    public IReadOnlyList<FaqItem> Faqs { get; private set; } = [];

    public void OnGet()
    {
        En = Lang.IsEnglish(Request);
        Faqs = FaqCatalog.Home(En);
        Seo.Apply(ViewData, Seo.Home(En));
    }
}
