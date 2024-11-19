namespace Shared.DataTransferObjects;

public record CreateCustomerRequestDto(string FirstName, string LastName, string Email, string Password, string? PhoneNumber, string? Address);
