using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtorNews.Models;
using RealtorNews.Services;

namespace RealtorNews.Controllers;

[Route("realtors")]
public class RealtorsController : Controller
{
    private readonly RealtorNewsStore _store;

    public RealtorsController(RealtorNewsStore store)
    {
        _store = store;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var model = new RealtorDirectoryViewModel();
        try
        {
            model.DbError = _store.Probe();
            if (model.DbError is null)
            {
                model.Realtors = _store.Directory(User.RealtorId());
            }
        }
        catch (Exception ex)
        {
            model.DbError = ex.Message;
        }

        return View(model);
    }

    [HttpGet("{slug}")]
    public IActionResult Profile(string slug)
    {
        var viewer = User.RealtorId();
        var realtor = _store.FindBySlug(slug, viewer);
        if (realtor is null)
        {
            return NotFound();
        }

        var model = new RealtorProfileViewModel
        {
            Realtor = realtor,
            Posts = _store.ForRealtor(realtor.Id, publishedOnly: true, viewer)
        };
        return View(model);
    }

    [Authorize]
    [HttpPost("{slug}/follow")]
    [ValidateAntiForgeryToken]
    public IActionResult Follow(string slug, string? returnUrl)
    {
        var viewer = User.RealtorId();
        var realtor = _store.FindBySlug(slug, viewer);
        if (viewer is null)
        {
            return Challenge();
        }

        if (realtor is not null)
        {
            _store.ToggleFollow(viewer.Value, realtor.Id);
        }

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Profile), new { slug });
    }
}
