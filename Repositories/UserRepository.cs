using Contracts.Repositories;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
namespace Repositories;

public class UserRepository : RepositoryBase<User>, IUser
{
    public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {}

    public void CreateUser(User user) => Create(user);

    public async Task<User?> GetUser(Guid userId, bool trackChanges) =>
        await FindByCondition(x => x.Id.Equals(userId), trackChanges).SingleOrDefaultAsync()!;

}