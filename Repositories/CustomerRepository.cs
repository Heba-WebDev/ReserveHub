using Contracts.Repositories;
using Entities.Models;
namespace Repositories;

public class CustomerRepository : RepositoryBase<Customer>, ICustomer
{
    public CustomerRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {}
}
