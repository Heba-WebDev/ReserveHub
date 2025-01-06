using Entities.Models;
using Repositories.Extensions.Utility;
using System.Linq.Dynamic.Core;
using System.Text;
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

    public static IQueryable<Customer> Sort(this IQueryable<Customer> customers, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return customers.OrderBy(e => e.FirstName);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Customer>(orderByQueryString);
        if (string.IsNullOrWhiteSpace(orderQuery))
            return customers.OrderBy(e => e.FirstName);
        return customers.OrderBy(orderQuery);
    }

}
