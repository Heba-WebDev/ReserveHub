using Entities.Models;
using Shared.RequestFeatures;
namespace Contracts.Repositories;

public interface ICustomer
{
    Task<PagedList<Customer>?> GetAllCustomers(CustomerParameters customerParameters, bool trackChanges);
    Task<Customer?> GetCustomer(Guid customerId, bool trackChanges);
    void CreateCustomer(Customer customer);
}
