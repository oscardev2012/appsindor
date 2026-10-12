using CalentonesAgua.Services;
using Microsoft.AspNetCore.Mvc;

namespace CalentonesAgua.ViewComponents;

public class CarritoResumenViewComponent : ViewComponent
{
    private readonly ICarritoService _carrito;

    public CarritoResumenViewComponent(ICarritoService carrito)
    {
        _carrito = carrito;
    }

    public IViewComponentResult Invoke()
    {
        return View(_carrito.ContarUnidades());
    }
}
