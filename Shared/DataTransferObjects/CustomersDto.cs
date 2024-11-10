namespace Shared.DataTransferObjects;

public record CustomersDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Address
    );
