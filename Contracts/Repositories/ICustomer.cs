using Entities.Models;
namespace Contracts.Repositories;

public interface ICustomer
{
    IEnumerable<Customer> GetAllCustomers(bool trackChanges);
}
