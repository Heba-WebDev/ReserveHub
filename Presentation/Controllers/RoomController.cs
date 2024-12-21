using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.Rooms;
namespace Presentation.Controllers;

[Route("/api/rooms")]
[ApiController]
public class RoomController : ControllerBase
{
    private readonly IServiceManager _service;
    public RoomController(IServiceManager service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequestDto createRoomRequestDto)
    {
        var createdRoom = await _service.RoomService.CreateRoom(createRoomRequestDto);
        return Ok(createdRoom);
    }

}
