using Shared.DataTransferObjects;
namespace Service.Contracts;

public interface ICustomerService
{
    IEnumerable<CustomersDto> GetAllCustomers(bool trackChanges);
}
