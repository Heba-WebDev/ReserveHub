using Entities.Models;
namespace Contracts.Repositories;

public interface ICustomer
{
    IEnumerable<Customer> GetAllCustomers(bool trackChanges);
    Customer GetCustomer(Guid customerId, bool trackChanges);
    void CreateCustomer(Customer customer);
}
