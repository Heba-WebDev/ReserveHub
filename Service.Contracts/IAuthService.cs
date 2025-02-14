using Shared.DataTransferObjects;
using Shared.DataTransferObjects.Auth;
namespace Service.Contracts;

public interface IAuthService
{
    Task<BasedResponseDto> Register(RegisterDto dto);
    Task<BasedResponseDto> Login(LoginDto dto, bool populateExp);
    Task<BasedResponseDto> RefreshToken (TokenDto dto);
}
