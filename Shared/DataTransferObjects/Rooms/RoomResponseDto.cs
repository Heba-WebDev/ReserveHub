using Entities.Enums;
namespace Shared.DataTransferObjects.Rooms;

public class RoomResponseDto
{
    public Guid Id { get; set; }
    public int Floor { get; set; }
    public int Number { get; set; }
    public RoomType RoomType { get; set; }
    public RoomStatus RoomStatus { get; set; }
    public decimal Price_Per_Night { get; set; }
}
