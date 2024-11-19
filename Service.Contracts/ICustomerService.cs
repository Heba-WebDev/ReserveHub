using Shared.DataTransferObjects;
namespace Service.Contracts;

public interface ICustomerService
{
    IEnumerable<CustomersDto> GetAllCustomers(bool trackChanges);
    CustomersDto GetCustomer(Guid CustomerId, bool trackChanges);
    CustomersDto CreateCustomer(CreateCustomerRequestDto customer);
}
