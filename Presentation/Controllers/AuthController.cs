using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.Auth;
namespace Presentation.Controllers;

[Route("/api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IServiceManager _service;
    public AuthController(IServiceManager service) => _service = service;

    [HttpPost("register")]
    public async Task<IActionResult> CreateUser([FromBody] RegisterDto dto)
    {
        var result = await _service.AuthService.Register(dto);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!await _service.UserService.ValidateUser(dto))
            return Unauthorized();
        var tokenDto = await _service.AuthService.Login(dto, populateExp: true);
        return Ok(tokenDto);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
    {
        var tokenDtoToReturn = await _service.AuthService.RefreshToken(tokenDto);
        return Ok(tokenDtoToReturn);
    }
}
