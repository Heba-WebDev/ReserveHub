using System.ComponentModel.DataAnnotations;
namespace Shared.DataTransferObjects;

public class CreateCustomerRequestDto
{
    [Required(ErrorMessage = "First name is required")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, contain at least one letter, one number, and one special character")]
    public required string Password { get; set; }

    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}
