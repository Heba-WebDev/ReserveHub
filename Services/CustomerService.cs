using AutoMapper;
using Contracts.Repositories;
using Entities.Exceptions;
using Entities.Models;
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

    public async Task<CustomersDto?> CreateCustomer(CreateCustomerRequestDto customer)
    {
        var customerEntity = _mapper.Map<Customer>(customer);
        _repository.Customer.CreateCustomer(customerEntity);
        await _repository.SaveAsync();
        var responseDto = _mapper.Map<CustomersDto>(customerEntity);
        return responseDto;
    }

    public async Task<IEnumerable<CustomersDto>?> GetAllCustomers(bool trackChanges)
    {
            var customers = await _repository.Customer.GetAllCustomers(trackChanges);
            var dto = _mapper.Map<IEnumerable<CustomersDto>>(customers);
            return dto;
    }

    public async Task<CustomersDto?> GetCustomer(Guid customerId, bool trackChanges)
    {
        var customer = await _repository.Customer.GetCustomer(customerId, trackChanges);
        if (customer == null)
        {
            throw new CustomerNotFoundException(customerId);
        }
        var dto = _mapper.Map<CustomersDto>(customer);
        return dto;
    }
}
