namespace CalentonesAgua.Models;

public class CarritoItem
{
    public int ProductoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Cantidad { get; set; }
    public string AcentoColor { get; set; } = "#c45c26";
    public string TipoEtiqueta { get; set; } = string.Empty;

    public decimal Subtotal => Precio * Cantidad;
}
