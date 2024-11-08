using Contracts;
using Contracts.Repositories;
using Service.Contracts;

namespace Services;

internal sealed class RoomService : IRoomService
{
    private readonly IRepositoryManager _repository;

    public RoomService(IRepositoryManager repository)
    {
        _repository = repository;
    }
}
