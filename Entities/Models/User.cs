using Microsoft.AspNetCore.Identity;
namespace Entities.Models;

public class User : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Address { get; set; }
    public Customer? Customers { get; set; }
}
