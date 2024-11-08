using Contracts;
using Contracts.Repositories;
using Service.Contracts;
namespace Services;

internal sealed class CustomerService : ICustomerService
{
    private readonly IRepositoryManager _repository;

    public CustomerService(IRepositoryManager repository)
    {
        _repository = repository;
    }
}
