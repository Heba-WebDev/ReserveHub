using Contracts;
using Contracts.Repositories;
using Service.Contracts;
namespace Services;

internal sealed class ReservationService : IRservationService
{
    private readonly IRepositoryManager _repository;

    public ReservationService(IRepositoryManager repository)
    {
        _repository = repository;
    }
}
