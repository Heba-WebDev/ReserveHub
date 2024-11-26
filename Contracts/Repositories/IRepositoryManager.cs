namespace Contracts.Repositories;

public interface IRepositoryManager
{
    IUser User { get; }
    ICustomer Customer { get; }
    IRoom Room { get; }
    IAmenity Amenity { get; }
    IRoomAmenity RoomAmenity { get; }
    IReservation Reservation { get; }
    void Save();
}
