using ReserveHub.Domain.Repositories;
namespace ReserveHub.Infrastructure.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<IRepositoryUser> _user;
    private readonly Lazy<IRepositoryHotel> _hotel;
    private readonly Lazy<IRepositoryRoom> _room;
    private readonly Lazy<IRepositoryReservation> _reservation;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _user = new Lazy<IRepositoryUser>(() => new UserRepository(repositoryContext));
        _room = new Lazy<IRepositoryRoom>(() => new RoomRepository(repositoryContext));
        _hotel = new Lazy<IRepositoryHotel>(() => new HotelRepository(repositoryContext));
        _reservation = new Lazy<IRepositoryReservation>(() => new ReservationRepository(repositoryContext));
    }

    public IRepositoryUser User => _user.Value;

    public IRepositoryHotel Hotel => _hotel.Value;

    public IRepositoryRoom Room => _room.Value;

    public IRepositoryReservation Reservation => _reservation.Value;

    public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
}