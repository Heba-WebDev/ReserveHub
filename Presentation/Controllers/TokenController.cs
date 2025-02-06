using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.Auth;
namespace Presentation.Controllers;

[Route("api/v1/token")]
[ApiController]
public class TokenController: ControllerBase
{
    private readonly IServiceManager _services;
    public TokenController(IServiceManager services) => _services = services;

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
    {
        var tokenDtoToReturn = await _services.UserService.RefreshToken(tokenDto);
        return Ok(tokenDtoToReturn);
    }
}
