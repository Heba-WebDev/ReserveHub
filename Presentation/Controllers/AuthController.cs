using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.Auth;
using Shared.DataTransferObjects.Users;
namespace Presentation.Controllers;

[Route("/api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IServiceManager _service;
    public AuthController(IServiceManager service) => _service = service;

    [HttpPost("register")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto user)
    {
        var result = await _service.UserService.CreateUser(user);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }
        return StatusCode(201);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!await _service.UserService.ValidateUser(dto))
            return Unauthorized();
        var tokenDto = await _service.UserService.CreateToken(populateExp: true);
        return Ok(tokenDto);
    }
}
