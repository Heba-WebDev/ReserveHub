using Microsoft.AspNetCore.Mvc;
using ReserveHub.Application.DTOs;
using ReserveHub.Application.DTOs.Auth;
using ReserveHub.Application.Services.Contracts;
namespace ReserveHub.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IServiceManager _manger;

    public AuthController(IServiceManager manager)
    {
        _manger = manager;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BaseResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _manger.AuthService.Register(dto);
        if (!result.Status)
            return StatusCode(StatusCodes.Status400BadRequest, result);

        return StatusCode(StatusCodes.Status201Created, result);
    }
}