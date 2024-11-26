using Contracts.Repositories;
using Entities.Models;
namespace Repositories;

public class UserRepository : RepositoryBase<User>, IUser
{
    public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {}

    public void CreateUser(User user) => Create(user);
}