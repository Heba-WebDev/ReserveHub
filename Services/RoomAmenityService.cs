using Contracts;
using Contracts.Repositories;
using Service.Contracts;
namespace Services;

internal sealed class RoomAmenityService : IRoomAmenityService
{
    private readonly IRepositoryManager _repository;

    public RoomAmenityService(IRepositoryManager repository)
    {
        _repository = repository;
    }
}
