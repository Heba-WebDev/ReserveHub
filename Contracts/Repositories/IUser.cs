using Entities.Models;
namespace Contracts.Repositories;

public interface IUser
{
    void CreateUser(User user);
    Task<User?> GetUser(Guid userId, bool trackChanges);
}
