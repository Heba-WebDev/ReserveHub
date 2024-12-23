namespace Shared.RequestFeatures;

public class RoomParameters : RequestParameters
{
    public int? FloorNumber { get; set; }
    public bool ValidFloorNumber => FloorNumber > 0 && FloorNumber < 5;
}
