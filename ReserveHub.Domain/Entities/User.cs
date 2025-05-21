namespace ReserveHub.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string? PhoneNumber { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public ICollection<Hotel> OwnedHotels { get; set; } = new List<Hotel>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
