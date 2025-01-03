using Entities.Models;

namespace Repositories.Extensions;

public static class RepositoryCustomerExtensions
{
    public static IQueryable<Customer> Search(this IQueryable<Customer> customers, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return customers;

        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return customers.Where(c =>
        c.FirstName.ToLower().Contains(searchTerm) ||
        c.LastName.ToLower().Contains(lowerCaseTerm) ||
        (c.FirstName + "" + c.LastName).ToLower().Contains(lowerCaseTerm));
    }
}
