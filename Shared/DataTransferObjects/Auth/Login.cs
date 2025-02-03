using System.ComponentModel.DataAnnotations;
namespace Shared.DataTransferObjects.Auth;

public class LoginDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, contain at least one letter, one number, and one special character")]
    public required string Password { get; set; }
}
