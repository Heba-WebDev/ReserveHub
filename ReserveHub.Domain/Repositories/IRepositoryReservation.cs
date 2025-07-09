using ReserveHub.Domain.Entities;
namespace ReserveHub.Domain.Repositories;

public interface IRepositoryReservation
{
    Task<Reservation?> GetReservationByIdAsync(Guid reservationId, bool trackChanges);
    Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(Guid userId, bool trackChanges);
    Task<IEnumerable<Reservation>> GetReservationsByRoomAsync(Guid roomId, bool trackChanges);
    Task CreateReservationAsync(Reservation reservation);
    Task UpdateReservationAsync(Reservation reservation);
    Task CancelReservationAsync(Guid reservationId);
}
