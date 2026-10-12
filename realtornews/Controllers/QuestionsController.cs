using Microsoft.AspNetCore.Mvc;
using RealtorNews.Data;
using RealtorNews.Models;
using RealtorNews.Services;

namespace RealtorNews.Controllers;

[Route("ask")]
public class QuestionsController : Controller
{
    private readonly RealtorNewsStore _store;

    public QuestionsController(RealtorNewsStore store)
    {
        _store = store;
    }

    [HttpGet("")]
    public IActionResult Index(string? topic)
        => View(new AskViewModel { Topic = LocalCatalog.AskTopics.Contains(topic) ? topic! : "Buying" });

    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public IActionResult Index(AskViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Body))
        {
            ModelState.AddModelError("", "Name, email, and question are required.");
            return View(model);
        }

        try
        {
            _store.AddQuestion(new PublicQuestion
            {
                FirstName = model.FirstName,
                Email = model.Email,
                Topic = LocalCatalog.AskTopics.Contains(model.Topic) ? model.Topic : "Buying",
                Body = model.Body
            });
            TempData["Saved"] = "Question received. A Charlotte Realtor can answer it from the studio.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }
}
