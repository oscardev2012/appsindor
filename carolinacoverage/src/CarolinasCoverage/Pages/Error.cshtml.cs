using System.Diagnostics;
using CarolinasCoverage.Site;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarolinasCoverage.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public ErrorModel(ILogger<ErrorModel> logger, SeoService seo)
    {
        _logger = logger;
        Seo = seo;
    }

    private readonly ILogger<ErrorModel> _logger;
    public SeoService Seo { get; }
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public int StatusCodeValue { get; set; } = 500;
    public bool En { get; private set; }

    public void OnGet(int code = 500)
    {
        StatusCodeValue = code is >= 400 and < 600 ? code : 500;
        Response.StatusCode = StatusCodeValue;
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        En = Lang.IsEnglish(Request);
        Seo.Apply(ViewData, Seo.Error(En, StatusCodeValue));
        _logger.LogWarning("Error page status {StatusCode} request {RequestId}", StatusCodeValue, RequestId);
    }
}
