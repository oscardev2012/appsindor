using ManuelaRuiz.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ManuelaRuiz.Web.Pages;

public sealed class AreaCityModel : PageModel
{
    public SeoCity City { get; private set; } = null!;

    public IActionResult OnGet(string slug)
    {
        var city = SeoGuide.Cities.FirstOrDefault(c =>
            string.Equals(c.Slug, slug, StringComparison.OrdinalIgnoreCase));
        if (city is null) return NotFound();
        City = city;
        return Page();
    }
}
