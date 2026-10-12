using CalentonesAgua.Data;
using CalentonesAgua.Models;
using CalentonesAgua.Services;
using Microsoft.AspNetCore.Mvc;

namespace CalentonesAgua.Controllers;

public class CarritoController : Controller
{
    private readonly ICarritoService _carrito;
    private readonly CatalogoProductos _catalogo;

    public CarritoController(ICarritoService carrito, CatalogoProductos catalogo)
    {
        _carrito = carrito;
        _catalogo = catalogo;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Carrito";
        var items = _carrito.ObtenerItems();
        ViewBag.Subtotal = _carrito.Subtotal();
        ViewBag.Envio = CarritoService.CalcularEnvio(_carrito.Subtotal());
        ViewBag.EnvioGratisDesde = CarritoService.EnvioGratisDesde;
        return View(items);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Agregar(int productoId, int cantidad = 1, string? returnUrl = null)
    {
        var producto = _catalogo.PorId(productoId);
        if (producto is null)
        {
            return NotFound();
        }

        _carrito.Agregar(producto, cantidad);
        TempData["Aviso"] = $"{producto.Nombre} se agregó al carrito.";

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Actualizar(int productoId, int cantidad)
    {
        _carrito.Actualizar(productoId, cantidad);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Quitar(int productoId)
    {
        _carrito.Quitar(productoId);
        TempData["Aviso"] = "Producto eliminado del carrito.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Checkout()
    {
        var items = _carrito.ObtenerItems();
        if (items.Count == 0)
        {
            TempData["Aviso"] = "Tu carrito está vacío.";
            return RedirectToAction(nameof(Index));
        }

        ViewData["Title"] = "Finalizar pedido";
        var subtotal = _carrito.Subtotal();
        var envio = CarritoService.CalcularEnvio(subtotal);
        var modelo = new CheckoutViewModel
        {
            Items = items,
            Subtotal = subtotal,
            Envio = envio,
            Instalacion = 0,
            Total = subtotal + envio
        };
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Checkout(CheckoutViewModel modelo)
    {
        var items = _carrito.ObtenerItems();
        if (items.Count == 0)
        {
            return RedirectToAction(nameof(Index));
        }

        var subtotal = _carrito.Subtotal();
        var envio = CarritoService.CalcularEnvio(subtotal);
        var instalacion = modelo.IncluirInstalacion ? CarritoService.CostoInstalacion : 0;

        modelo.Items = items;
        modelo.Subtotal = subtotal;
        modelo.Envio = envio;
        modelo.Instalacion = instalacion;
        modelo.Total = subtotal + envio + instalacion;

        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Finalizar pedido";
            return View(modelo);
        }

        var folio = $"CH-{DateTime.Now:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
        _carrito.Vaciar();
        TempData["Folio"] = folio;
        TempData["Total"] = modelo.Total.ToString("N2");
        TempData["Nombre"] = modelo.Nombre;
        return RedirectToAction(nameof(Confirmacion));
    }

    public IActionResult Confirmacion()
    {
        if (TempData["Folio"] is null)
        {
            return RedirectToAction("Index", "Home");
        }

        ViewData["Title"] = "Pedido recibido";
        return View();
    }
}
