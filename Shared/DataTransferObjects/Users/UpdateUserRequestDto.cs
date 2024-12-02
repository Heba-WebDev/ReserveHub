using System.ComponentModel.DataAnnotations;
namespace Shared.DataTransferObjects.Users;

public class UpdateUserRequestDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }

    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, contain at least one letter, one number, and one special character")]
    public string? Password { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }
}
