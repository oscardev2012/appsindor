using Microsoft.AspNetCore.Mvc;
using RealtorNews.Models;
using RealtorNews.Services;

namespace RealtorNews.Controllers;

public class HomeController : Controller
{
    private readonly RealtorNewsStore _store;

    public HomeController(RealtorNewsStore store)
    {
        _store = store;
    }

    public IActionResult Index()
    {
        var viewer = User.RealtorId();
        var model = new HomePageViewModel();
        try
        {
            model.DbError = _store.Probe();
            if (model.DbError is null)
            {
                model.Latest = _store.Published(viewerId: viewer, take: 6);
                model.Articles = _store.Published(type: "article", viewerId: viewer, take: 4);
                model.Realtors = _store.Featured(3, viewer);
                model.RealtorCount = _store.RealtorCount();
                model.PostCount = _store.PublishedCount();
            }
        }
        catch (Exception ex)
        {
            model.DbError = ex.Message;
        }

        return View(model);
    }

    [HttpGet("/for-realtors")]
    public IActionResult ForRealtors()
    {
        ViewBag.DbError = SafeProbe();
        ViewBag.RealtorCount = SafeCount();
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        => View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });

    private string? SafeProbe()
    {
        try
        {
            return _store.Probe();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private int SafeCount()
    {
        try
        {
            return _store.RealtorCount();
        }
        catch
        {
            return 0;
        }
    }
}
