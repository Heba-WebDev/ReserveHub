using System.ComponentModel.DataAnnotations;
using ReserveHub.Application.Validators;
namespace ReserveHub.Application.DTOs.Hotel;

public class CreateHotelDto
{
    [MaxLength(50, ErrorMessage = "Hotel name can not exceed 50 charachters")]
    [MinLength(3, ErrorMessage = "Hotel name must be of minimum 3 characters")]
    public string Name { get; set; } = "";
    [MaxLength(120, ErrorMessage ="Description can not exceed 120 character")]
    public string Description { get; set; } = "";
    [Required]
    [ValidCountry]
    public string Country { get; set; } = string.Empty;
    [Required(ErrorMessage = "Hotel Location/Address is required")]
    [MinLength(12, ErrorMessage ="A minimum of 12 charachter long location is required")]
    public string Location { get; set; } = "";
}