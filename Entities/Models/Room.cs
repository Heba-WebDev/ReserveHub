using System.ComponentModel.DataAnnotations;
using Entities.Enums;
namespace Entities.Models;

public class Room
{
    public Guid Id {get; set;}
    [Required(ErrorMessage = "A room number is required")]
    public int Number {get; set;}
    [Required(ErrorMessage = "A floor number is required")]
    public int Floor {get; set;}
    [Required(ErrorMessage = "A room type is required")]
    public RoomType Type {get; set;}
    [Required(ErrorMessage = "A room status is required")]
    public RoomStatus Status {get; set;}
    [Required(ErrorMessage = "A price per night is required")]
    public decimal Price_Per_Night {get; set;}
    public ICollection<RoomAmenity> RoomAmenities {get; set;} = new List<RoomAmenity>();
}
