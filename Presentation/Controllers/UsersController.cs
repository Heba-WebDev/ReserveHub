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
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto user)
    {
        if (user is null)
            return BadRequest("Request body can not be empty");
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var createdUser = await _service.UserService.CreateUser(user);
        return CreatedAtRoute("UserById", new { id = createdUser.Id }, createdUser);
    }

    [HttpGet("{id:guid}", Name = "UserById")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _service.UserService.GetUserById(id, trackChanges: false);
        return Ok(user);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequestDto updateUserRequestDto)
    {
        if (updateUserRequestDto is null)
        {
            return BadRequest("Body request is empty");
        }
        await _service.UserService.UpdateUser(id, updateUserRequestDto, trackChanges: true);
        return NoContent();
    }
}
