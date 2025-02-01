using AutoMapper;
using Contracts;
using Contracts.Repositories;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using Shared.DataTransferObjects;
namespace Services;
public class ServiceManager : IServiceManager
{
    private readonly Lazy<IUserService> _usersService;
    private readonly Lazy<ICustomerService> _customersService;
    private readonly Lazy<IRoomService> _roomService;
    private readonly Lazy<IRservationService> _reservationService;
    private readonly Lazy<IAmenityService> _amenityService;
    private readonly Lazy<IRoomAmenityService> _roomAmenityService;
    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, IDataShaper<CustomersDto> dataShaper, IConfiguration configuration, UserManager<User> userManager)
    {
        _usersService = new Lazy<IUserService>(() => new UserService(repositoryManager, mapper, userManager, configuration));
        _customersService = new Lazy<ICustomerService>(() => new CustomerService(repositoryManager, mapper, dataShaper));
        _roomService = new Lazy<IRoomService>(() => new RoomService(repositoryManager, mapper));
        _reservationService = new Lazy<IRservationService>(() => new ReservationService(repositoryManager));
        _amenityService = new Lazy<IAmenityService>(() => new AmenityService(repositoryManager));
        _roomAmenityService = new Lazy<IRoomAmenityService>(() => new RoomAmenityService(repositoryManager));
    }
    public IUserService UserService => _usersService.Value;
    public ICustomerService CustomerService => _customersService.Value;

    public IRoomService RoomService => _roomService.Value;

    public IAmenityService AmenityService => _amenityService.Value;

    public IRoomAmenityService RoomAmenityService => _roomAmenityService.Value;

    public IRservationService RservationService => _reservationService.Value;
}
