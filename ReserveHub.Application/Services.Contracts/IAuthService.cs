using ReserveHub.Application.DTOs;
using ReserveHub.Application.DTOs.Auth;
namespace ReserveHub.Application.Services.Contracts;

public interface IAuthService
{
    Task<BaseResponseDto> Register(RegisterDto dto);
    Task<BaseResponseDto> Login(LoginDto dto);
    Task<BaseResponseDto> RefreshToken(TokenDto dto);
    Task<BaseResponseDto> ConfirmEmailAsync(string email, string token);
    Task<BaseResponseDto> ResendConfirmationEmailAsync(string email);
    Task<BaseResponseDto> SendPasswordResetEmailAsync(string email);
    Task<BaseResponseDto> ResetPasswordAsync(ResetPasswordDto dto);
}
