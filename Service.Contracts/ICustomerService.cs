using Shared.DataTransferObjects;
namespace Service.Contracts;

public interface ICustomerService
{
    Task<IEnumerable<CustomersDto>?> GetAllCustomers(bool trackChanges);
    Task<CustomersDto?> GetCustomer(Guid CustomerId, bool trackChanges);
    Task<CustomersDto?> CreateCustomer(CreateCustomerRequestDto customer);
}
