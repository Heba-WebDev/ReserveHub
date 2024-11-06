using Entities.Enums;

namespace Entities.Models;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ReservationStatus ReservationStatus { get; set; }
}