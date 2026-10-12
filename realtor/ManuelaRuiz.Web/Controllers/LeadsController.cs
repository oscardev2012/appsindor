using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using ManuelaRuiz.Web.Services;

namespace ManuelaRuiz.Web.Controllers;

[Route("leads")]
public sealed class LeadsController : Controller
{
    private readonly LeadStore _store;

    public LeadsController(LeadStore store) => _store = store;

    [HttpPost("search")]
    [ValidateAntiForgeryToken]
    public IActionResult Search(IFormCollection form) => Save("search", form);

    [HttpPost("intake")]
    [ValidateAntiForgeryToken]
    public IActionResult Intake(IFormCollection form) => Save("intake", form);

    [HttpPost("qualify")]
    [ValidateAntiForgeryToken]
    public IActionResult Qualify(IFormCollection form) => Save("qualify", form);

    [HttpPost("program")]
    [ValidateAntiForgeryToken]
    public IActionResult Program(IFormCollection form) => Save("program", form);

    private IActionResult Save(string kind, IFormCollection form)
    {
        var fields = form
            .Where(pair => pair.Key is not "__RequestVerificationToken")
            .ToDictionary(
                pair => pair.Key,
                pair => string.Join(", ", pair.Value.ToArray()),
                StringComparer.OrdinalIgnoreCase);

        var name = fields.GetValueOrDefault("name") ?? "";
        var email = fields.GetValueOrDefault("email") ?? "";
        var phone = fields.GetValueOrDefault("phone") ?? "";

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone))
        {
            TempData["LeadKind"] = kind;
            TempData["LeadOk"] = IsEnglish
                ? "Please complete name, email and phone."
                : "Completa nombre, email y teléfono.";
            TempData["LeadError"] = "1";
            return RedirectBack();
        }

        _store.Add(new Lead(DateTimeOffset.Now, kind, name.Trim(), email.Trim(), phone.Trim(), fields));
        TempData["LeadKind"] = kind;
        TempData["LeadOk"] = IsEnglish
            ? "Received. Manuela will review your inquiry and follow up. This is not a loan application."
            : "Recibido. Manuela revisará tu consulta y te contactará. Esto no es una solicitud de préstamo.";
        return RedirectBack();
    }

    private bool IsEnglish =>
        CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);

    private IActionResult RedirectBack()
    {
        var referer = Request.Headers.Referer.ToString();
        if (string.IsNullOrWhiteSpace(referer)) return Redirect("/");
        if (!Uri.TryCreate(referer, UriKind.Absolute, out var uri)) return Redirect("/");
        return Redirect(uri.PathAndQuery);
    }
}
