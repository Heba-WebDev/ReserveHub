using AutoMapper;
using Contracts;
using Contracts.Repositories;
using Service.Contracts;
namespace Services;
public class ServiceManager : IServiceManager
{
    private readonly Lazy<ICustomerService> _customersService;
    private readonly Lazy<IRoomService> _roomService;
    private readonly Lazy<IRservationService> _reservationService;
    private readonly Lazy<IAmenityService> _amenityService;
    private readonly Lazy<IRoomAmenityService> _roomAmenityService;
    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _customersService = new Lazy<ICustomerService>(() => new CustomerService(repositoryManager, mapper));
        _roomService = new Lazy<IRoomService>(() => new RoomService(repositoryManager));
        _reservationService = new Lazy<IRservationService>(() => new ReservationService(repositoryManager));
        _amenityService = new Lazy<IAmenityService>(() => new AmenityService(repositoryManager));
        _roomAmenityService = new Lazy<IRoomAmenityService>(() => new RoomAmenityService(repositoryManager));
    }
    public ICustomerService CustomerService => _customersService.Value;

    public IRoomService RoomService => _roomService.Value;

    public IAmenityService AmenityService => _amenityService.Value;

    public IRoomAmenityService RoomAmenityService => _roomAmenityService.Value;

    public IRservationService RservationService => _reservationService.Value;
}
