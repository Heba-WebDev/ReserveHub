using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Service.Contracts;
using Shared.DataTransferObjects.Rooms;
using Shared.RequestFeatures;
namespace Presentation.Controllers;

[Route("/api/v{v:apiversion}/rooms")]
[ApiController]
[EnableRateLimiting("Fixed")]
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

    [HttpGet]
    public async Task<IActionResult> GetAllRooms([FromQuery] RoomParameters roomParameters)
    {
        var (rooms, metaData) = await _service.RoomService.GetAllRooms(roomParameters, trackChanges: false);
        var paginationHeader = JsonSerializer.Serialize(metaData);
        Response.Headers.Append("X-Pagination", paginationHeader);
        return Ok(new
        {
            Rooms = rooms,
            MetaData = metaData
        });
    }

    [HttpGet("{roomId:guid}")]
    public async Task<IActionResult> GetRoomById(Guid roomId)
    {
        var room = await _service.RoomService.GetRoomById(roomId, trackChanges: false);
        return Ok(room);
    }
}
