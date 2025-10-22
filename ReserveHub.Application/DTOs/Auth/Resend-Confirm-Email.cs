using System.ComponentModel.DataAnnotations;
namespace ReserveHub.Application.DTOs.Auth;

public class ResendConfirmationDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }
}

