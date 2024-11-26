using Entities.Models;
namespace Contracts.Repositories;

public interface IUser
{
    void CreateUser(User user);
}
