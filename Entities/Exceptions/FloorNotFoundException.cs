namespace Entities.Exceptions;

public class FloorNotFoundException : NotFoundException
{
    public FloorNotFoundException(int? FloorNumber): base($"floor number {FloorNumber} does not exsit")
    {}
}
