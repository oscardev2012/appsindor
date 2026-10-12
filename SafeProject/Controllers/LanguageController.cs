using CalentonesAgua.Data;
using Microsoft.AspNetCore.Mvc;

namespace CalentonesAgua.Controllers;

public class LanguageController : Controller
{
    private readonly SeoCatalog _catalog;

    public LanguageController(SeoCatalog catalog) => _catalog = catalog;

    public IActionResult Set(string? code, string? returnUrl)
    {
        var lang = SiteLanguage.Normalize(code);
        SiteLanguage.SetCookie(Response, lang);
        var dest = SiteLanguage.RedirectAfterChoice(lang, returnUrl ?? "/", _catalog);
        return LocalRedirect(dest);
    }
}
