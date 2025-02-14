namespace Shared.DataTransferObjects.Users;

public record UserDto
{
    public Guid Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
    public string? RefreshToken { get; init; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}