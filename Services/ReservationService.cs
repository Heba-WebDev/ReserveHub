using Contracts;
using Contracts.Repositories;
using Service.Contracts;
namespace Services;

internal sealed class ReservationService : IRservationService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public ReservationService(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
