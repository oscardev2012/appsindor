using System.ComponentModel.DataAnnotations;

namespace CalentonesAgua.Models;

public class ContactoViewModel
{
    [Required(ErrorMessage = "Escribe tu nombre.")]
    [Display(Name = "Nombre")]
    [StringLength(80)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Necesitamos un correo para responderte.")]
    [EmailAddress(ErrorMessage = "El correo no es válido.")]
    [Display(Name = "Correo")]
    public string Correo { get; set; } = string.Empty;

    [Phone(ErrorMessage = "El teléfono no es válido.")]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "Cuéntanos en qué podemos ayudarte.")]
    [Display(Name = "Mensaje")]
    [StringLength(1200, MinimumLength = 10, ErrorMessage = "El mensaje debe tener al menos 10 caracteres.")]
    public string Mensaje { get; set; } = string.Empty;
}
