using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Controllers;

public class Auth : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("all-users")]
    public IActionResult AllUsers()
    {
        Log.Information("My log");
        return Ok(new
            {
            status = "success",
            message = "All users"
            }
        );
    }
}