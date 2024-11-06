
namespace Entities.Models;

public class RoomAmenity
{
    public int Id { get; set;}
    public Guid RoomId { get; set;}
    public Room Room { get; set;} = null!;
    public Guid AmenityId { get; set; }
    public Amenity Amenity { get; set; } = null!;
}