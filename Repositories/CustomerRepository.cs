using Contracts.Repositories;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
using Repositories.Extensions;
namespace Repositories;

public class CustomerRepository : RepositoryBase<Customer>, ICustomer
{
    public CustomerRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {}

    public async Task<PagedList<Customer>?> GetAllCustomers(CustomerParameters customerParameters, bool trackChanges)
    {
        var totalCustomers = await FindAll(trackChanges).CountAsync();
        var customers = await FindAll(trackChanges)
        .Search(customerParameters.SearchTerm!)
        .OrderBy(x => x.FirstName)
        .Skip((customerParameters.PageNumber - 1) * customerParameters.PageSize)
        .Take(customerParameters.PageSize)
        .ToListAsync();
        return PagedList<Customer>
            .ToPagedList(customers, totalCustomers, customerParameters.PageNumber, customerParameters.PageSize);
    }

    public async Task<Customer?> GetCustomer(Guid CustomerId, bool trackChanges) =>
    await FindByCondition(c => c.Id.Equals(CustomerId), trackChanges).SingleOrDefaultAsync()!;

    public void CreateCustomer(Customer customer) => Create(customer);
}
