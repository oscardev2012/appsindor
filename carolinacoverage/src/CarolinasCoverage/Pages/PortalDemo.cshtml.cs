using CarolinasCoverage.Site;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarolinasCoverage.Pages;

public class PortalDemoModel : PageModel
{
    public PortalDemoModel(SeoService seo) => Seo = seo;

    public SeoService Seo { get; }
    public bool En { get; private set; }

    public void OnGet()
    {
        En = Lang.IsEnglish(Request);
        Seo.Apply(ViewData, Seo.Portal(En));
    }
}
