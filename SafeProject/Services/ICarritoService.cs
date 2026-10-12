using CalentonesAgua.Models;

namespace CalentonesAgua.Services;

public interface ICarritoService
{
    IReadOnlyList<CarritoItem> ObtenerItems();
    void Agregar(Producto producto, int cantidad = 1);
    void Actualizar(int productoId, int cantidad);
    void Quitar(int productoId);
    void Vaciar();
    int ContarUnidades();
    decimal Subtotal();
}
