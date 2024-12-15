using Entities.Models;
namespace Contracts.Repositories;

public interface ICustomer
{
    Task<IEnumerable<Customer>?> GetAllCustomers(bool trackChanges);
    Task<Customer?> GetCustomer(Guid customerId, bool trackChanges);
    void CreateCustomer(Customer customer);
}
