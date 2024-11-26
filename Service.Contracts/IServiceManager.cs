namespace Service.Contracts;

public interface IServiceManager
{
    IUserService UserService { get; }
    ICustomerService CustomerService { get; }
    IRoomService RoomService { get; }
    IAmenityService AmenityService { get; }
    IRoomAmenityService RoomAmenityService { get; }
    IRservationService RservationService { get; }
}
