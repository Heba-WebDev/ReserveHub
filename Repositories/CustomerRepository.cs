using Contracts.Repositories;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
namespace Repositories;

public class CustomerRepository : RepositoryBase<Customer>, ICustomer
{
    public CustomerRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {}

    public async Task<IEnumerable<Customer>?> GetAllCustomers(bool trackChanges) =>
    await FindAll(trackChanges)
    .OrderBy(x => x.FirstName)
    .ToListAsync();

    public async Task<Customer?> GetCustomer(Guid CustomerId, bool trackChanges) =>
    await FindByCondition(c => c.Id.Equals(CustomerId), trackChanges).SingleOrDefaultAsync()!;

    public void CreateCustomer(Customer customer) => Create(customer);
}
