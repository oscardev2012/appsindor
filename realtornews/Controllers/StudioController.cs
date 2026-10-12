using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtorNews.Models;
using RealtorNews.Services;

namespace RealtorNews.Controllers;

[Authorize]
[Route("studio")]
public class StudioController : Controller
{
    private readonly RealtorNewsStore _store;

    public StudioController(RealtorNewsStore store)
    {
        _store = store;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var realtor = Current();
        if (realtor is null)
        {
            return Challenge();
        }

        var model = new StudioHomeViewModel
        {
            Realtor = realtor,
            Posts = _store.ForRealtor(realtor.Id, publishedOnly: false, realtor.Id),
            Questions = _store.OpenQuestions()
        };
        return View(model);
    }

    [HttpGet("compose")]
    public IActionResult Compose(string? title)
        => View(new ComposeViewModel { Title = title ?? "" });

    [HttpGet("edit/{id:guid}")]
    public IActionResult Edit(Guid id)
    {
        var realtor = Current();
        var post = _store.GetPost(id, realtor?.Id);
        if (realtor is null || post is null || post.RealtorId != realtor.Id)
        {
            return NotFound();
        }

        return View("Compose", new ComposeViewModel
        {
            Id = post.Id,
            PostType = post.PostType,
            Category = post.Category,
            Title = post.Title,
            Summary = post.Summary ?? "",
            Body = post.Body,
            CoverImageUrl = post.CoverImageUrl ?? "",
            City = post.City ?? "Charlotte",
            Publish = post.IsPublished
        });
    }

    [HttpPost("compose")]
    [ValidateAntiForgeryToken]
    public IActionResult Compose(ComposeViewModel model)
    {
        var realtor = Current();
        if (realtor is null)
        {
            return Challenge();
        }

        if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Body))
        {
            ModelState.AddModelError("", "El título y el texto de la noticia son obligatorios.");
            return View(model);
        }

        var post = model.Id is Guid id
            ? _store.GetPost(id, realtor.Id)
            : new NewsPost { RealtorId = realtor.Id };
        if (post is null || (model.Id is not null && post.RealtorId != realtor.Id))
        {
            return NotFound();
        }

        post.RealtorId = realtor.Id;
        post.PostType = model.PostType;
        post.Category = model.Category;
        post.Title = model.Title;
        post.Summary = model.Summary;
        post.Body = model.Body;
        post.CoverImageUrl = model.CoverImageUrl;
        post.City = model.City;
        var saved = _store.SavePost(post, model.Publish);
        if (saved.IsPublished)
        {
            TempData["Saved"] = "Noticia publicada. Ya aparece en el menú Noticias y en el sitio.";
            return RedirectToAction("Details", "News", new { slug = saved.Slug });
        }

        TempData["Saved"] = "Borrador guardado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(Guid id)
    {
        var realtor = Current();
        if (realtor is null)
        {
            return Challenge();
        }

        _store.DeletePost(id, realtor.Id);
        TempData["Saved"] = "Post removed.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("profile")]
    public IActionResult Profile()
    {
        var realtor = Current();
        if (realtor is null)
        {
            return Challenge();
        }

        return View(ToRegister(realtor));
    }

    [HttpPost("profile")]
    [ValidateAntiForgeryToken]
    public IActionResult Profile(RegisterViewModel model)
    {
        var realtor = Current();
        if (realtor is null)
        {
            return Challenge();
        }

        realtor.FullName = model.FullName.Trim();
        realtor.Headline = NullIfEmpty(model.Headline);
        realtor.Bio = NullIfEmpty(model.Bio);
        realtor.Phone = NullIfEmpty(model.Phone);
        realtor.Brokerage = NullIfEmpty(model.Brokerage);
        realtor.LicenseNumber = NullIfEmpty(model.LicenseNumber);
        realtor.City = NullIfEmpty(model.City);
        realtor.State = NullIfEmpty(model.State);
        realtor.Languages = NullIfEmpty(model.Languages);
        _store.UpdateProfile(realtor);
        TempData["Saved"] = "Profile updated.";
        return RedirectToAction(nameof(Index));
    }

    private RealtorAccount? Current()
    {
        var id = User.RealtorId();
        return id is null ? null : _store.FindById(id.Value);
    }

    private static RegisterViewModel ToRegister(RealtorAccount realtor)
        => new()
        {
            FullName = realtor.FullName,
            Email = realtor.Email,
            Brokerage = realtor.Brokerage ?? "",
            LicenseNumber = realtor.LicenseNumber ?? "",
            City = realtor.City ?? "Charlotte",
            State = realtor.State ?? "NC",
            Phone = realtor.Phone ?? "",
            Languages = realtor.Languages ?? "English",
            Headline = realtor.Headline ?? "",
            Bio = realtor.Bio ?? ""
        };

    private static string? NullIfEmpty(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
