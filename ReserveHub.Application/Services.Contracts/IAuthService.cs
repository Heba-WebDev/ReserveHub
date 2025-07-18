using ReserveHub.Application.DTOs;
using ReserveHub.Application.DTOs.Auth;
namespace ReserveHub.Application.Services.Contracts;

public interface IAuthService
{
    Task<BasedResponseDto> Register(RegisterDto dto);
    Task<BasedResponseDto> Login(LoginDto dto);
    Task<BasedResponseDto> RefreshToken(TokenDto dto);
}
