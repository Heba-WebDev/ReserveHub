using ReserveHub.Domain.Enums;
namespace ReserveHub.Domain.Entities;

public class Room
{
    public Guid Id { get; set; }
    public RoomType Type { get; set; }
    public RoomStatus Status { get; set; }
    public decimal Price_Per_Night { get; set; }
    public Guid HotelId { get; set; }
    public Hotel Hotel { get; set; } = new Hotel();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
