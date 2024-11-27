using Contracts.Repositories;
using Entities.Models;
namespace Repositories;

public class UserRepository : RepositoryBase<User>, IUser
{
    public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {}

    public void CreateUser(User user) => Create(user);

    public User GetUser(Guid userId, bool trackChanges) =>
        FindByCondition(x => x.Id.Equals(userId), trackChanges).SingleOrDefault()!;

}