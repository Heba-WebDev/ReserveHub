using AutoMapper;
using Contracts.Repositories;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;
namespace Services;

internal sealed class CustomerService : ICustomerService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public CustomerService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public IEnumerable<CustomersDto> GetAllCustomers(bool trackChanges)
    {
            var customers = _repository.Customer.GetAllCustomers(trackChanges);
            var dto = _mapper.Map<IEnumerable<CustomersDto>>(customers);
            return dto;
    }

    public CustomersDto GetCustomer(Guid customerId, bool trackChanges)
    {
        var customer = _repository.Customer.GetCustomer(customerId, trackChanges);
        if (customer == null)
        {
            throw new CustomerNotFoundException(customerId);
        }
        var dto = _mapper.Map<CustomersDto>(customer);
        return dto;
    }
}
