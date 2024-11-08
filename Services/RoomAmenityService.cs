using Contracts;
using Contracts.Repositories;
using Service.Contracts;
namespace Services;

internal sealed class RoomAmenityService : IRoomAmenityService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public RoomAmenityService(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
