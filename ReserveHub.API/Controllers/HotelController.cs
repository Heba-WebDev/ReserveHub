using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReserveHub.Application.DTOs;
using ReserveHub.Application.DTOs.Hotel;
using ReserveHub.Application.Services.Contracts;

namespace ReserveHub.API.Controllers;

[ApiController]
[Route("api/hotel")]
public class HotelController : ControllerBase
{
    private readonly ICurrentUserService _currentUser;
    private readonly IServiceManager _manager;

    public HotelController(ICurrentUserService currentUserService, IServiceManager serviceManager)
    {
        Console.WriteLine("[HotelController] Constructor called - Controller is being instantiated");
        _currentUser = currentUserService;
        _manager = serviceManager;
    }

    [HttpGet("test")]
    [AllowAnonymous]
    public IActionResult Test()
    {
        Console.WriteLine("[HotelController.Test] Test endpoint reached!");
        return Ok(new { message = "Hotel controller is working", timestamp = DateTime.UtcNow });
    }

    [HttpPost]
    [Route("create")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BaseResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CreateHotelDto dto)
    {
        Console.WriteLine("[HotelController.Create] Endpoint reached!");
        Console.WriteLine($"[HotelController.Create] Request received with DTO: Name={dto?.Name}, Country={dto?.Country}");
        
        if (dto == null)
        {
            return BadRequest(new BaseResponseDto { Status = false, Message = "Request body is required" });
        }
        
        if (!ModelState.IsValid)
        {
            Console.WriteLine("[HotelController.Create] ModelState is invalid");
            Console.WriteLine($"[HotelController.Create] ModelState errors: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
            return BadRequest(ModelState);
        }

        Console.WriteLine("[HotelController.Create] Calling HotelService.Create");
        var result = await _manager.HotelService.Create(dto);
        Console.WriteLine($"[HotelController.Create] Service returned Status: {result.Status}, Message: {result.Message}");

        if (!result.Status)
            return StatusCode(StatusCodes.Status400BadRequest, result);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet("{hotelId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string hotelId)
    {
        if (!Guid.TryParse(hotelId, out _))
            return BadRequest(new BaseResponseDto
            {
                Status = false,
                Message = "Invalid hotel ID format"
            });

        var result = await _manager.HotelService.GetHotelById(hotelId);

        if (!result.Status)
            return StatusCode(StatusCodes.Status404NotFound, result);

        return Ok(result);
    }

    [HttpPut]
    [Route("{hotelId}")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string hotelId, UpdateHotelDto dto)
    {
        if (!Guid.TryParse(hotelId, out _))
            return BadRequest(new BaseResponseDto
            {
                Status = false,
                Message = "Invalid hotel ID format"
            });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _manager.HotelService.Update(hotelId, dto);

        if (!result.Status)
        {
            if (result.Message?.Contains("not found") == true)
                return StatusCode(StatusCodes.Status404NotFound, result);
            if (result.Message?.Contains("unauthorized") == true)
                return StatusCode(StatusCodes.Status403Forbidden, result);
            
            return StatusCode(StatusCodes.Status400BadRequest, result);
        }

        return Ok(result);
    }

    [HttpDelete]
    [Route("{hotelId}")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string hotelId)
    {
        if (!Guid.TryParse(hotelId, out _))
            return BadRequest(new BaseResponseDto
            {
                Status = false,
                Message = "Invalid hotel ID format"
            });

        if (_currentUser.UserId == null)
            return StatusCode(StatusCodes.Status401Unauthorized, new BaseResponseDto
            {
                Status = false,
                Message = "User not authenticated"
            });

        var result = await _manager.HotelService.Delete(hotelId, _currentUser.UserId);

        if (!result.Status)
        {
            if (result.Message?.Contains("not found") == true)
                return StatusCode(StatusCodes.Status404NotFound, result);
            if (result.Message?.Contains("unauthorized") == true)
                return StatusCode(StatusCodes.Status403Forbidden, result);
            
            return StatusCode(StatusCodes.Status400BadRequest, result);
        }

        return Ok(result);
    }
}