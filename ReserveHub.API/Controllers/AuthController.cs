using Microsoft.AspNetCore.Mvc;
using ReserveHub.Application.DTOs;
using ReserveHub.Application.DTOs.Auth;
using ReserveHub.Application.Services.Contracts;
namespace ReserveHub.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IServiceManager _manager;

    public AuthController(IServiceManager manager)
    {
        _manager = manager;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BaseResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _manager.AuthService.Register(dto);
        if (!result.Status)
            return StatusCode(StatusCodes.Status400BadRequest, result);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _manager.AuthService.Login(dto);

        if (!result.Status)
        {
            if (result.Data != null && result.Data.GetType().GetProperty("EmailConfirmed") != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, result);
            }

            return StatusCode(StatusCodes.Status400BadRequest, result);
        }

        return Ok(result);
    }

    [HttpPost("confirm-email")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _manager.AuthService.ConfirmEmailAsync(dto.Email, dto.Token);

        if (!result.Status)
        {
            if (result.Message!.Contains("User not found"))
                return StatusCode(StatusCodes.Status404NotFound, result);

            return StatusCode(StatusCodes.Status400BadRequest, result);
        }

        return Ok(result);
    }

    [HttpPost("resend-confirmation")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResendConfirmation(ResendConfirmationDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _manager.AuthService.ResendConfirmationEmailAsync(dto.Email);

        if (!result.Status)
        {
            if (result.Message!.Contains("User not found"))
                return StatusCode(StatusCodes.Status404NotFound, result);

            return StatusCode(StatusCodes.Status400BadRequest, result);
        }

        return Ok(result);
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _manager.AuthService.SendPasswordResetEmailAsync(dto.Email);

        if (!result.Status)
        {
            if (result.Message!.Contains("User not found"))
                return StatusCode(StatusCodes.Status404NotFound, result);

            return StatusCode(StatusCodes.Status400BadRequest, result);
        }

        return Ok(result);
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _manager.AuthService.ResetPasswordAsync(dto);

        if (!result.Status)
        {
            if (result.Message!.Contains("User not found"))
                return StatusCode(StatusCodes.Status404NotFound, result);

            return StatusCode(StatusCodes.Status400BadRequest, result);
        }

        return Ok(result);
    }
}
