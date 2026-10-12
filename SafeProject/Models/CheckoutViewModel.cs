using System.ComponentModel.DataAnnotations;

namespace CalentonesAgua.Models;

public class CheckoutViewModel
{
    [Required(ErrorMessage = "Escribe tu nombre.")]
    [Display(Name = "Nombre completo")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no es válido.")]
    [Display(Name = "Correo")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Display(Name = "Teléfono")]
    [Phone]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "Indica la dirección de entrega.")]
    [Display(Name = "Dirección")]
    [StringLength(200)]
    public string Direccion { get; set; } = string.Empty;

    [Required(ErrorMessage = "Indica la ciudad.")]
    [Display(Name = "Ciudad")]
    public string Ciudad { get; set; } = string.Empty;

    [Display(Name = "Código postal")]
    public string? CodigoPostal { get; set; }

    [Display(Name = "Notas para la entrega")]
    [StringLength(400)]
    public string? Notas { get; set; }

    [Display(Name = "Incluir instalación profesional")]
    public bool IncluirInstalacion { get; set; }

    public IReadOnlyList<CarritoItem> Items { get; set; } = Array.Empty<CarritoItem>();
    public decimal Subtotal { get; set; }
    public decimal Envio { get; set; }
    public decimal Instalacion { get; set; }
    public decimal Total { get; set; }
}
