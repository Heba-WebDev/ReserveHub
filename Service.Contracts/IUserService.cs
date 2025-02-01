using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects.Users;
namespace Service.Contracts;

public interface IUserService
{
    Task<IdentityResult> CreateUser(CreateUserRequestDto user);
    Task<UserDto> GetUserById(Guid userId, bool trackChanges);
    Task UpdateUser(Guid userId, UpdateUserRequestDto user, bool trackChanges);
}
