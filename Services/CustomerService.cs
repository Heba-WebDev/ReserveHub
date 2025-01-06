using System.Dynamic;
using AutoMapper;
using Contracts;
using Contracts.Repositories;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
namespace Services;

internal sealed class CustomerService : ICustomerService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IDataShaper<CustomersDto> _dataShaper;

    public CustomerService(IRepositoryManager repository, IMapper mapper, IDataShaper<CustomersDto> dataShaper)
    {
        _repository = repository;
        _mapper = mapper;
        _dataShaper = dataShaper;
    }

    public async Task<CustomersDto?> CreateCustomer(CreateCustomerRequestDto customer)
    {
        var customerEntity = _mapper.Map<Customer>(customer);
        _repository.Customer.CreateCustomer(customerEntity);
        await _repository.SaveAsync();
        var responseDto = _mapper.Map<CustomersDto>(customerEntity);
        return responseDto;
    }

    public async Task<(IEnumerable<ExpandoObject>? customers, MetaData metaData)> GetAllCustomers(CustomerParameters customerParameters, bool trackChanges)
    {
            var customers = await _repository.Customer.GetAllCustomers(customerParameters, trackChanges);
            var dto = _mapper.Map<IEnumerable<CustomersDto>>(customers);
            var shapedData = _dataShaper.ShapeData(dto, customerParameters.Fields!);
            return (customers: shapedData, customers!.MetaData);
    }

    public async Task<CustomersDto?> GetCustomer(Guid customerId, bool trackChanges)
    {
        var customer = await GetCustomerAndCheckIfItExists(customerId, trackChanges);
        var dto = _mapper.Map<CustomersDto>(customer);
        return dto;
    }

    private async Task<Customer?> GetCustomerAndCheckIfItExists(Guid customerId, bool trackChanges)
    {
        var customer = await _repository.Customer.GetCustomer(customerId, trackChanges);
        if (customer == null)
        {
            throw new CustomerNotFoundException(customerId);
        }
        return customer;
    }
}
