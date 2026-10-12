namespace CalentonesAgua.Models;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Marca { get; set; } = "CalorHogar";
    public TipoCalenton Tipo { get; set; }
    public string Resumen { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public decimal? PrecioAnterior { get; set; }
    public int CapacidadLitros { get; set; }
    public string Combustible { get; set; } = string.Empty;
    public int PersonasRecomendadas { get; set; }
    public int GarantiaAnios { get; set; } = 3;
    public bool EnOferta { get; set; }
    public bool Destacado { get; set; }
    public bool EnStock { get; set; } = true;
    public string AcentoColor { get; set; } = "#c45c26";
    public IReadOnlyList<string> Caracteristicas { get; set; } = Array.Empty<string>();

    public string TipoEtiqueta => Tipo switch
    {
        TipoCalenton.GasPaso => "Gas de paso",
        TipoCalenton.GasDeposito => "Gas de depósito",
        TipoCalenton.Electrico => "Eléctrico",
        TipoCalenton.Solar => "Solar",
        TipoCalenton.Hibrido => "Híbrido",
        _ => "Calentón"
    };

    public bool TieneDescuento => PrecioAnterior.HasValue && PrecioAnterior.Value > Precio;
}
