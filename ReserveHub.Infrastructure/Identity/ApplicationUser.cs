using Microsoft.AspNetCore.Identity;
using ReserveHub.Domain.Entities;
namespace ReserveHub.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public required string FullName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string? GoogleId { get; set; }
    public ICollection<Hotel> OwnedHotels { get; set; } = new List<Hotel>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
