using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects.Auth;
using Shared.DataTransferObjects.Users;
namespace Service.Contracts;

public interface IUserService
{
    Task<bool> ValidateUser(LoginDto dto);
    Task<UserDto> GetUserById(Guid userId, bool trackChanges);
    Task UpdateUser(Guid userId, UpdateUserRequestDto user, bool trackChanges);
}
