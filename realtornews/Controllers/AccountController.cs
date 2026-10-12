using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RealtorNews.Models;
using RealtorNews.Services;

namespace RealtorNews.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private static readonly PasswordHasher<RealtorAccount> Hasher = new();
    private readonly RealtorNewsStore _store;

    public AccountController(RealtorNewsStore store)
    {
        _store = store;
    }

    [HttpGet("register")]
    public IActionResult Register()
        => View(new RegisterViewModel());

    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.FullName) || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
        {
            ModelState.AddModelError("", "Nombre, email y contraseña son obligatorios.");
        }

        if ((model.Password ?? "").Length < 8)
        {
            ModelState.AddModelError(nameof(model.Password), "Usa al menos 8 caracteres.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            if (_store.FindByEmail(model.Email) is not null)
            {
                ModelState.AddModelError(nameof(model.Email), "Ese email ya está registrado.");
                return View(model);
            }

            var realtor = new RealtorAccount
            {
                Email = model.Email,
                FullName = model.FullName.Trim(),
                Brokerage = NullIfEmpty(model.Brokerage),
                LicenseNumber = NullIfEmpty(model.LicenseNumber),
                City = NullIfEmpty(model.City) ?? "Charlotte",
                State = NullIfEmpty(model.State) ?? "NC",
                Phone = NullIfEmpty(model.Phone),
                Languages = NullIfEmpty(model.Languages) ?? "English",
                Headline = NullIfEmpty(model.Headline) ?? "Charlotte-area Realtor",
                Bio = NullIfEmpty(model.Bio)
            };
            realtor.PasswordHash = Hasher.HashPassword(realtor, model.Password!);
            realtor = _store.Register(realtor);
            await SignInAsync(realtor);
            return RedirectToAction("Index", "Studio");
        }
        catch (SqlException ex) when (ex.Number is 208 or 207)
        {
            ModelState.AddModelError("", "Falta crear las tablas nuevas. Ejecuta Data/realtor-news.sql en IndorDB y vuelve a intentar.");
            return View(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [HttpGet("login")]
    public IActionResult Login(string? returnUrl = null)
        => View(new LoginViewModel { ReturnUrl = returnUrl });

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        try
        {
            var realtor = _store.FindByEmail(model.Email ?? "");
            var valid = realtor is not null
                && Hasher.VerifyHashedPassword(realtor, realtor.PasswordHash, model.Password ?? "")
                    != PasswordVerificationResult.Failed;
            if (!valid || realtor is null)
            {
                ModelState.AddModelError("", "Email o contraseña incorrectos.");
                return View(model);
            }

            _store.TouchLogin(realtor.Id);
            await SignInAsync(realtor);
            if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Studio");
        }
        catch (SqlException ex) when (ex.Number is 208 or 207)
        {
            ModelState.AddModelError("", "Falta crear las tablas nuevas. Ejecuta Data/realtor-news.sql en IndorDB y vuelve a intentar.");
            return View(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [Authorize]
    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    private async Task SignInAsync(RealtorAccount realtor)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, realtor.Id.ToString()),
            new(ClaimTypes.Name, realtor.FullName),
            new(ClaimTypes.Email, realtor.Email),
            new("slug", realtor.Slug)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = true });
    }

    private static string? NullIfEmpty(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
