using Shared.DataTransferObjects.Users;
namespace Service.Contracts;

public interface IUserService
{
    Task<UserDto> CreateUser(CreateUserRequestDto user);
    Task<UserDto> GetUserById(Guid userId, bool trackChanges);
    Task UpdateUser(Guid userId, UpdateUserRequestDto user, bool trackChanges);
}
