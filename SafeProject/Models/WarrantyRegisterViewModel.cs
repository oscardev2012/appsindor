using System.ComponentModel.DataAnnotations;

namespace CalentonesAgua.Models;

public class WarrantyRegisterViewModel
{
    [Required, Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required, Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, Display(Name = "Mobile phone")]
    public string Phone { get; set; } = string.Empty;

    [Required, Display(Name = "Street address")]
    public string Address { get; set; } = string.Empty;

    [Display(Name = "Unit / apartment")]
    public string? Unit { get; set; }

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = "North Carolina";

    [Required, Display(Name = "ZIP code")]
    public string Zip { get; set; } = string.Empty;

    [Required, Display(Name = "Installation date")]
    public DateTime? InstallationDate { get; set; }

    [Required]
    public string Manufacturer { get; set; } = string.Empty;

    [Required, Display(Name = "Model number")]
    public string ModelNumber { get; set; } = string.Empty;

    [Required, Display(Name = "Serial number")]
    public string SerialNumber { get; set; } = string.Empty;

    [Required, Display(Name = "Tank size")]
    public string TankSize { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;

    [Display(Name = "Invoice or work order")]
    public string? Invoice { get; set; }

    [Display(Name = "Installing company / technician")]
    public string? Technician { get; set; }

    [Display(Name = "Anything SAFE should know?")]
    public string? Notes { get; set; }

    public bool AuthorizeContact { get; set; }

    [Range(typeof(bool), "true", "true", ErrorMessage = "Please certify the information is accurate.")]
    public bool Certify { get; set; }
}
