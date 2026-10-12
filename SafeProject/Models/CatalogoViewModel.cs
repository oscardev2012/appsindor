namespace CalentonesAgua.Models;

public class CatalogoViewModel
{
    public IReadOnlyList<Producto> Productos { get; set; } = Array.Empty<Producto>();
    public string? Tipo { get; set; }
    public string? Busqueda { get; set; }
    public string Orden { get; set; } = "destacados";
    public int Total { get; set; }
}
