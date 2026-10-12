using System.ComponentModel.DataAnnotations;

namespace CalentonesAgua.Models;

public class SafePlanViewModel
{
    [Required, Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required, Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required, Display(Name = "Mobile phone")]
    public string Phone { get; set; } = string.Empty;

    [Required, Display(Name = "Installation address")]
    public string Address { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = "North Carolina";

    [Required, Display(Name = "ZIP code")]
    public string Zip { get; set; } = string.Empty;

    [Required, Display(Name = "Do you own the property?")]
    public string OwnProperty { get; set; } = string.Empty;

    [Required, Display(Name = "Current type")]
    public string CurrentType { get; set; } = string.Empty;

    [Required, Display(Name = "Tank size")]
    public string TankSize { get; set; } = string.Empty;

    [Required, Display(Name = "When do you need replacement?")]
    public string Timing { get; set; } = string.Empty;

    [Required, Display(Name = "Is the current tank leaking?")]
    public string Leaking { get; set; } = string.Empty;

    public bool AuthorizeContact { get; set; }

    [Range(typeof(bool), "true", "true", ErrorMessage = "Please confirm you understand this is a preliminary request.")]
    public bool UnderstandTerms { get; set; }
}
