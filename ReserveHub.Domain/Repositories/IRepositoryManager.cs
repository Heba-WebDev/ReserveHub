using ReserveHub.Domain.Entities;
namespace ReserveHub.Domain.Repositories;

public interface IRepositoryManager
{
    IRepositoryUser User { get; }
    IRepositoryHotel Hotel { get; }
    IRepositoryRoom Room { get; }
    IRepositoryReservation Reservation { get; }
    Task SaveAsync();
}
