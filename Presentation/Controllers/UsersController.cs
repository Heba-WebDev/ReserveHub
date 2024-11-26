using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.DataTransferObjects.Users;
namespace Presentation.Controllers;

[Route("/api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IServiceManager _service;
    public UsersController(IServiceManager service) => _service = service;

    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserRequestDto user)
    {
        if (user is null)
            return BadRequest("Request body can not be empty");
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var createdUser = _service.UserService.CreateUser(user);
        return Ok(new { createdUser });
    }
}