using System.Text.Json;
using CalentonesAgua.Models;

namespace CalentonesAgua.Services;

public class CarritoService : ICarritoService
{
    public const decimal EnvioGratisDesde = 8000m;
    public const decimal CostoEnvio = 249m;
    public const decimal CostoInstalacion = 890m;

    private const string SessionKey = "carrito";
    private readonly IHttpContextAccessor _http;

    public CarritoService(IHttpContextAccessor http)
    {
        _http = http;
    }

    public IReadOnlyList<CarritoItem> ObtenerItems() => Leer();

    public void Agregar(Producto producto, int cantidad = 1)
    {
        var items = Leer();
        var existente = items.FirstOrDefault(i => i.ProductoId == producto.Id);
        if (existente is null)
        {
            items.Add(new CarritoItem
            {
                ProductoId = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Cantidad = Math.Max(1, cantidad),
                AcentoColor = producto.AcentoColor,
                TipoEtiqueta = producto.TipoEtiqueta
            });
        }
        else
        {
            existente.Cantidad += Math.Max(1, cantidad);
        }

        Guardar(items);
    }

    public void Actualizar(int productoId, int cantidad)
    {
        var items = Leer();
        var item = items.FirstOrDefault(i => i.ProductoId == productoId);
        if (item is null)
        {
            return;
        }

        if (cantidad <= 0)
        {
            items.Remove(item);
        }
        else
        {
            item.Cantidad = Math.Min(cantidad, 10);
        }

        Guardar(items);
    }

    public void Quitar(int productoId)
    {
        var items = Leer();
        items.RemoveAll(i => i.ProductoId == productoId);
        Guardar(items);
    }

    public void Vaciar() => Guardar(new List<CarritoItem>());

    public int ContarUnidades() => Leer().Sum(i => i.Cantidad);

    public decimal Subtotal() => Leer().Sum(i => i.Subtotal);

    public static decimal CalcularEnvio(decimal subtotal) =>
        subtotal >= EnvioGratisDesde || subtotal == 0 ? 0 : CostoEnvio;

    private List<CarritoItem> Leer()
    {
        var json = _http.HttpContext?.Session.GetString(SessionKey);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<CarritoItem>();
        }

        return JsonSerializer.Deserialize<List<CarritoItem>>(json) ?? new List<CarritoItem>();
    }

    private void Guardar(List<CarritoItem> items)
    {
        _http.HttpContext?.Session.SetString(SessionKey, JsonSerializer.Serialize(items));
    }
}
