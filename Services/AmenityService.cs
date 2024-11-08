using Contracts;
using Contracts.Repositories;
using Service.Contracts;
namespace Services;

internal sealed class AmenityService : IAmenityService
{
    private readonly IRepositoryManager _repository;

    public AmenityService(IRepositoryManager repository)
    {
        _repository = repository;
    }
}
