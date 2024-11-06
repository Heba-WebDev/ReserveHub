using System.ComponentModel.DataAnnotations;
namespace Entities.Models;
public class Amenity
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "A room id is required")]
    public string Amenity_name { get; set; } = "";
    public ICollection<RoomAmenity> RoomAmenities { get; set;} = new List<RoomAmenity>();
}
