using ReserveHub.Application.DTOs;
using ReserveHub.Application.DTOs.Auth;
namespace ReserveHub.Application.Services.Contracts;

public interface IAuthService
{
    Task<BaseResponseDto> Register(RegisterDto dto);
    Task<BaseResponseDto> Login(LoginDto dto);
    Task<BaseResponseDto> RefreshToken(TokenDto dto);
}
