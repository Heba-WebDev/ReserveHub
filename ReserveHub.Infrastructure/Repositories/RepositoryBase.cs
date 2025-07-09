using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ReserveHub.Domain.Repositories;
using ReserveHub.Infrastructure.Repositories;
namespace ReserveHub.Infrastructure.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected RepositoryContext RepositoryContext;
    public RepositoryBase(RepositoryContext repositoryContext)
        => RepositoryContext = repositoryContext;
    public async Task Create(T entity) =>
        await RepositoryContext.Set<T>().AddAsync(entity);

    public async Task Update(T entity)
    {
        RepositoryContext.Set<T>().Update(entity);
        await Task.CompletedTask;
    }

    public async Task Delete(T entity)
    {
        RepositoryContext.Set<T>().Remove(entity);
        await Task.CompletedTask;
    }

    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ?
            RepositoryContext.Set<T>().AsNoTracking()
        :
            RepositoryContext.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
        !trackChanges ?
            RepositoryContext.Set<T>()
            .Where(expression)
            .AsNoTracking()
        :
            RepositoryContext.Set<T>()
            .Where(expression);
}
