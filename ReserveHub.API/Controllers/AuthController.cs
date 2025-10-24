using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ReserveHub.Application.DTOs;
using ReserveHub.Application.DTOs.Auth;
using ReserveHub.Application.Services.Contracts;
using ReserveHub.Infrastructure.Configurations;
using ReserveHub.Infrastructure.Services;
namespace ReserveHub.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IServiceManager _manager;
    private readonly IOptions<FrontendConfiguration> _frontendConfiguration;

    public AuthController(IServiceManager manager, IOptions<FrontendConfiguration> frontendConfiguration)
    {
        _manager = manager;
        _frontendConfiguration = frontendConfiguration;
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
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _manager.AuthService.ConfirmEmailAsync(dto.Email, dto.Token);

       return result.Status ? Ok(result) : StatusCode(StatusCodes.Status400BadRequest, result);
    }

    [HttpPost("resend-confirmation")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResendConfirmation(ResendConfirmationDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _manager.AuthService.ResendConfirmationEmailAsync(dto.Email);

        if (!result.Status)
        {
 
            return StatusCode(StatusCodes.Status400BadRequest, result);
        }

        return result.Status ? Ok(result) : StatusCode(StatusCodes.Status400BadRequest, result);
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _manager.AuthService.SendPasswordResetEmailAsync(dto.Email);

        return result.Status ? Ok(result) : StatusCode(StatusCodes.Status400BadRequest, result);
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _manager.AuthService.ResetPasswordAsync(dto);

        return result.Status ? Ok(result) : StatusCode(StatusCodes.Status400BadRequest, result);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken(TokenDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _manager.AuthService.RefreshToken(dto);

        return result.Status ? Ok(result) : StatusCode(StatusCodes.Status400BadRequest, result);
    }

    [HttpGet("signin-google")]
    public IActionResult GoogleLogin()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleCallback")
        };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        
        if (!result.Succeeded)
            return BadRequest("Google authentication failed");
        
        var claims = result.Principal.Claims.ToList();
        var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        var name = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        var googleId = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        
        var authResult = await _manager.GoogleAuthService.HandleGoogleCallbackAsync(email, name, googleId);
        
        if (authResult.Status)
        {
            // Extract the TokenDto from the Data property
            var tokenDto = authResult.Data as dynamic;
            var accessToken = tokenDto?.AccessToken;
            return Redirect($"{_frontendConfiguration.Value.Url}/auth/callback?token={accessToken}");
        }
        
        return BadRequest(authResult);
    }
}
