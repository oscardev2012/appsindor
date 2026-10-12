using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtorNews.Data;
using RealtorNews.Models;
using RealtorNews.Services;

namespace RealtorNews.Controllers;

[Route("feed")]
public class FeedController : Controller
{
    private readonly RealtorNewsStore _store;

    public FeedController(RealtorNewsStore store)
    {
        _store = store;
    }

    [HttpGet("")]
    public IActionResult Index(string? category, string? scope)
    {
        var viewer = User.RealtorId();
        var model = new FeedPageViewModel
        {
            Category = string.IsNullOrWhiteSpace(category) ? "all" : category,
            Scope = string.Equals(scope, "following", StringComparison.OrdinalIgnoreCase) ? "following" : "all"
        };

        try
        {
            model.DbError = _store.Probe();
            if (model.DbError is null)
            {
                model.Posts = model.Scope == "following" && viewer is Guid id
                    ? _store.FollowingFeed(id)
                    : _store.Published(model.Category, viewerId: viewer);
                model.Experts = _store.Featured(4, viewer);
            }
        }
        catch (Exception ex)
        {
            model.DbError = ex.Message;
        }

        ViewBag.Categories = PostCatalog.Categories;
        return View(model);
    }

    [Authorize]
    [HttpPost("{id:guid}/like")]
    [ValidateAntiForgeryToken]
    public IActionResult Like(Guid id, string? returnUrl)
    {
        var viewer = User.RealtorId();
        if (viewer is null)
        {
            return Challenge();
        }

        _store.ToggleLike(id, viewer.Value);
        return LocalBack(returnUrl);
    }

    [Authorize]
    [HttpPost("{id:guid}/comment")]
    [ValidateAntiForgeryToken]
    public IActionResult Comment(Guid id, string body, string? returnUrl)
    {
        var viewer = User.RealtorId();
        if (viewer is null)
        {
            return Challenge();
        }

        if (!string.IsNullOrWhiteSpace(body))
        {
            _store.AddComment(id, viewer.Value, body);
        }

        return LocalBack(returnUrl);
    }

    private IActionResult LocalBack(string? returnUrl)
        => !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? Redirect(returnUrl)
            : RedirectToAction(nameof(Index));
}
