using ReserveHub.Domain.Entities;
using ReserveHub.Domain.Repositories;
using ReserveHub.Infrastructure.Identity;
namespace ReserveHub.Infrastructure.Repositories;

class UserRepository : RepositoryBase<ApplicationUser>, IRepositoryUser
{
    public UserRepository(RepositoryContext repositoryContext): base(repositoryContext)
    {}
    public Task CreateUserAsync(User user)
    {
        throw new NotImplementedException();
    }
    public Task DeleteUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetUserByEmailAsync(string email, bool trackChanges)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetUserByIdAsync(Guid userId, bool trackChanges)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserAsync(User user)
    {
        throw new NotImplementedException();
    }
}