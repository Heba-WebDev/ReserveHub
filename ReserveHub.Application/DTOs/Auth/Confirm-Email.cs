using System.ComponentModel.DataAnnotations;
namespace ReserveHub.Application.DTOs.Auth;

public class ConfirmEmailDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }
    
    [Required(ErrorMessage = "Confirmation token is required")]
    public required string Token { get; set; }
}

