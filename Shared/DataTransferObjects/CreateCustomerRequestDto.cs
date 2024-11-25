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
    public Guid? UserId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}
