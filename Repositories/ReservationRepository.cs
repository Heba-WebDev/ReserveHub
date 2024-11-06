using Contracts.Repositories;
using Entities.Models;
namespace Repositories;

public class ReservationRepository : RepositoryBase<Reservation>, IReservation
{
    public ReservationRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    { }
}
