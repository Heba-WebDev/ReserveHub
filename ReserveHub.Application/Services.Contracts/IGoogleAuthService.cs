using ReserveHub.Application.DTOs;
namespace ReserveHub.Application.Services.Contracts;

public interface IGoogleAuthService
{
    Task<BaseResponseDto> HandleGoogleCallbackAsync(string? email, string? name, string? googleId);
}
