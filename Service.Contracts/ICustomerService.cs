using Shared.DataTransferObjects;
using Shared.RequestFeatures;
namespace Service.Contracts;

public interface ICustomerService
{
    Task<(IEnumerable<CustomersDto>? customers, MetaData metaData)> GetAllCustomers(CustomerParameters customerParameters, bool trackChanges);
    Task<CustomersDto?> GetCustomer(Guid CustomerId, bool trackChanges);
    Task<CustomersDto?> CreateCustomer(CreateCustomerRequestDto customer);
}
