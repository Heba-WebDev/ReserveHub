using Contracts.Repositories;
namespace Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<ICustomer> _customer;
    private readonly Lazy<IRoom> _room;
    private readonly Lazy<IAmenity> _amenity;
    private readonly Lazy<IRoomAmenity> _roomAmenity;
    private readonly Lazy<IReservation> _reservation;
    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _customer = new Lazy<ICustomer>(() => new CustomerRepository(repositoryContext));
        _room = new Lazy<IRoom>(() => new RoomRepository(repositoryContext));
        _amenity = new Lazy<IAmenity>(() => new AmenityRepository(repositoryContext));
        _roomAmenity = new Lazy<IRoomAmenity>(() => new RoomAmenityRepository(repositoryContext));
        _reservation = new Lazy<IReservation>(() => new ReservationRepository(repositoryContext));
    }
    public ICustomer Customer => _customer.Value;

    public IRoom Room => _room.Value;

    public IAmenity Amenity => _amenity.Value;

    public IRoomAmenity RoomAmenity => _roomAmenity.Value;

    public IReservation Reservation => _reservation.Value;

    public void Save() => _repositoryContext.SaveChanges();
}