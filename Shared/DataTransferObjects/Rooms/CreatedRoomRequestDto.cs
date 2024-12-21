using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects.Rooms;

public class CreateRoomRequestDto
{
    [Required(ErrorMessage = "A room number is required")]
    public required int Number { get; set; }
    [Required(ErrorMessage = "A floor number is required")]
    public required int Floor { get; set; }
    [Required(ErrorMessage = "A room type is required")]
    public required string Type { get; set; }
    public string Status { get; set; } = "Vacant";
    [Required(ErrorMessage = "A price per night is required")]
    public required decimal Price_Per_Night { get; set; }
}