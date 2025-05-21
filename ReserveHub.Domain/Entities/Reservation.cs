using ReserveHub.Domain.Enums;
namespace ReserveHub.Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public string UserId { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ReservationStatus Status { get; set; }
    public Room Room { get; set; } = new Room();
}