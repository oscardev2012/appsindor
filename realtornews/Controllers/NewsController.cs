using Microsoft.AspNetCore.Mvc;
using RealtorNews.Data;
using RealtorNews.Models;
using RealtorNews.Services;

namespace RealtorNews.Controllers;

[Route("news")]
public class NewsController : Controller
{
    private readonly RealtorNewsStore _store;

    public NewsController(RealtorNewsStore store)
    {
        _store = store;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var viewer = User.RealtorId();
        var model = new MagazinePageViewModel();
        try
        {
            model.DbError = _store.Probe();
            if (model.DbError is null)
            {
                var articles = _store.Published(type: "article", viewerId: viewer, take: 12);
                var mixed = articles.Count > 0 ? articles : _store.Published(viewerId: viewer, take: 12);
                model.Lead = mixed.FirstOrDefault();
                model.Stories = mixed.Skip(1).ToList();
            }
        }
        catch (Exception ex)
        {
            model.DbError = ex.Message;
        }

        return View(model);
    }

    [HttpGet("preview")]
    public IActionResult Preview()
    {
        var post = new NewsPost
        {
            PostType = "article",
            Category = "Rates",
            Title = "Qué hacer antes de cambiar el presupuesto por las tasas",
            Summary = "Una tasa más alta no cierra la puerta. Cambia el pago mensual y el tipo de casa que encaja sin presión.",
            Body = NewsFormatter.SampleBody,
            CoverImageUrl = "/img/hero-charlotte.jpg",
            City = "Charlotte",
            IsPublished = true,
            PublishedUtc = DateTimeOffset.UtcNow,
            RealtorName = "Ejemplo editorial",
            RealtorSlug = "preview",
            RealtorHeadline = "Así se lee una noticia en el sitio"
        };
        return View("Details", new PostDetailViewModel { Post = post });
    }

    [HttpGet("{slug}")]
    public IActionResult Details(string slug)
    {
        var viewer = User.RealtorId();
        var post = _store.GetPostBySlug(slug, viewer);
        if (post is null || (!post.IsPublished && post.RealtorId != viewer))
        {
            return NotFound();
        }

        var model = new PostDetailViewModel
        {
            Post = post,
            Comments = _store.Comments(post.Id),
            More = _store.Published(viewerId: viewer, take: 5)
                .Where(p => p.Id != post.Id)
                .Take(3)
                .ToList()
        };
        return View(model);
    }
}
