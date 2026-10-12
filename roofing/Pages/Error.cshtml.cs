using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MajesticRoofing.Pages;

public class ErrorModel : PageModel
{
    public int StatusCodeValue { get; private set; } = 500;
    public bool IsNotFound => StatusCodeValue == 404;

    public void OnGet(int code = 500)
    {
        StatusCodeValue = code is >= 400 and < 600 ? code : 500;
        Response.StatusCode = StatusCodeValue;
        Response.Headers["X-Robots-Tag"] = "noindex, nofollow";
        ViewData["Lang"] = "en";
    }
}
