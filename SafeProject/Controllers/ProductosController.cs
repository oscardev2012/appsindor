using CalentonesAgua.Data;
using CalentonesAgua.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalentonesAgua.Controllers;

public class ProductosController : Controller
{
    private readonly CatalogoProductos _catalogo;

    public ProductosController(CatalogoProductos catalogo)
    {
        _catalogo = catalogo;
    }

    public IActionResult Index(string? tipo, string? q, string orden = "destacados")
    {
        var productos = _catalogo.Filtrar(tipo, q, orden);
        var modelo = new CatalogoViewModel
        {
            Productos = productos,
            Tipo = tipo,
            Busqueda = q,
            Orden = orden,
            Total = productos.Count
        };

        ViewData["Title"] = "Catálogo";
        return View(modelo);
    }

    public IActionResult Detalle(string id)
    {
        var producto = _catalogo.PorSlug(id);
        if (producto is null)
        {
            return NotFound();
        }

        ViewData["Title"] = producto.Nombre;
        ViewBag.Relacionados = _catalogo.Relacionados(producto);
        return View(producto);
    }
}
