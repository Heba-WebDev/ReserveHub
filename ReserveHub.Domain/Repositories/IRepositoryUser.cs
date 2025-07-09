using ReserveHub.Domain.Entities;
namespace ReserveHub.Domain.Repositories;

public interface IRepositoryUser
{
    Task CreateUserAsync(User user);
    Task<User?> GetUserByIdAsync(Guid userId, bool trackChanges);
    Task<User?> GetUserByEmailAsync(string email, bool trackChanges);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(User user);
}
