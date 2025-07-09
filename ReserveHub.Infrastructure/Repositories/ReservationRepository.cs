using ReserveHub.Domain.Entities;
using ReserveHub.Domain.Repositories;
namespace ReserveHub.Infrastructure.Repositories;

public class ReservationRepository : RepositoryBase<Reservation>, IRepositoryReservation
{
    public ReservationRepository(RepositoryContext repositoryContext): base(repositoryContext) {}
    public Task CancelReservationAsync(Guid reservationId)
    {
        throw new NotImplementedException();
    }

    public Task CreateReservationAsync(Reservation reservation)
    {
        throw new NotImplementedException();
    }

    public Task<Reservation?> GetReservationByIdAsync(Guid reservationId, bool trackChanges)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Reservation>> GetReservationsByRoomAsync(Guid roomId, bool trackChanges)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(Guid userId, bool trackChanges)
    {
        throw new NotImplementedException();
    }

    public Task UpdateReservationAsync(Reservation reservation)
    {
        throw new NotImplementedException();
    }
}