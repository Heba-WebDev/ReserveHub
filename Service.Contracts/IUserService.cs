using Shared.DataTransferObjects.Users;
namespace Service.Contracts;

public interface IUserService
{
    UserDto CreateUser(CreateUserRequestDto user);
    UserDto GetUserById(Guid userId, bool trackChanges);
}
