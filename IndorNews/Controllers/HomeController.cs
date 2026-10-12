using System.Diagnostics;
using IndorNews.Data;
using IndorNews.Models;
using IndorNews.Services;
using Microsoft.AspNetCore.Mvc;

namespace IndorNews.Controllers;

public class HomeController : Controller
{
    private readonly SitePostStore _store;
    private readonly IConfiguration _config;

    public HomeController(SitePostStore store, IConfiguration config)
    {
        _store = store;
        _config = config;
    }

    public IActionResult Index()
    {
        MarkPrivate("Publicar noticias");
        if (!SitePostAdminAuth.IsSignedIn(Request, _config))
        {
            return RedirectToAction(nameof(Login));
        }

        try
        {
            return View(new PublishPageViewModel { Posts = _store.All() });
        }
        catch (Exception ex)
        {
            ViewBag.DbError = ex.Message;
            return View(new PublishPageViewModel());
        }
    }

    [HttpGet]
    public IActionResult Login()
    {
        MarkPrivate("Entrar");
        if (SitePostAdminAuth.IsSignedIn(Request, _config))
        {
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Configured = SitePostAdminAuth.Token(_config) is not null;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string password)
    {
        MarkPrivate("Entrar");
        var expected = _config["Updates:AdminPassword"];
        if (string.IsNullOrWhiteSpace(expected) || !string.Equals(password, expected, StringComparison.Ordinal))
        {
            ViewBag.Configured = SitePostAdminAuth.Token(_config) is not null;
            ViewBag.Error = "La contraseña no es correcta.";
            return View();
        }

        Response.Cookies.Append(SitePostAdminAuth.CookieName, SitePostAdminAuth.Token(_config)!, new CookieOptions
        {
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.Lax,
            Secure = Request.IsHttps,
            Expires = DateTimeOffset.UtcNow.AddDays(14),
        });
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(SitePostAdminAuth.CookieName);
        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(string site, string title, string body, string? imageUrl)
    {
        MarkPrivate("Publicar noticias");
        if (!SitePostAdminAuth.IsSignedIn(Request, _config))
        {
            return RedirectToAction(nameof(Login));
        }

        var page = LoadPage(site, title, body, imageUrl);

        if (!SitePostCatalog.TryNormalize(site, out _))
        {
            TempData["Error"] = "Elige un sitio web.";
            return View(nameof(Index), page);
        }

        var copyError = PostCopyLimits.Validate(title, body);
        if (copyError is not null)
        {
            TempData["Error"] = copyError;
            return View(nameof(Index), page);
        }

        try
        {
            _store.Add(site, title, body ?? "", imageUrl);
            TempData["Saved"] = "Publicada. Ese sitio mostrará esta noticia hasta que publiques otra más nueva para el mismo sitio.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = "No se pudo guardar en IndorDB: " + ex.Message;
            return View(nameof(Index), page);
        }
    }

    private PublishPageViewModel LoadPage(string? site, string? title, string? body, string? imageUrl)
    {
        IReadOnlyList<SitePost> posts = [];
        try
        {
            posts = _store.All();
        }
        catch (Exception ex)
        {
            ViewBag.DbError = ex.Message;
        }

        return new PublishPageViewModel
        {
            Posts = posts,
            Site = site,
            Title = title ?? "",
            Body = body ?? "",
            ImageUrl = imageUrl,
        };
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private void MarkPrivate(string title)
    {
        ViewData["Title"] = title;
        ViewData["Robots"] = "noindex, nofollow";
    }
}
