using CalentonesAgua.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalentonesAgua.Controllers;

public class ContactoController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Contacto";
        return View(new ContactoViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(ContactoViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Contacto";
            return View(modelo);
        }

        TempData["Nombre"] = modelo.Nombre;
        return RedirectToAction(nameof(Enviado));
    }

    public IActionResult Enviado()
    {
        ViewData["Title"] = "Mensaje enviado";
        return View();
    }
}
