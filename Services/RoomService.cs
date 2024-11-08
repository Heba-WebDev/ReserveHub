using Contracts;
using Contracts.Repositories;
using Service.Contracts;

namespace Services;

internal sealed class RoomService : IRoomService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public RoomService(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
