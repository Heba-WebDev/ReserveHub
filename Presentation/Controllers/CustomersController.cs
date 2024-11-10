using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
namespace Presentation.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IServiceManager _service;
    public CustomersController(IServiceManager service) => _service = service;

    [HttpGet]
    public IActionResult GetCustomers()
    {
        try
        {
            var customers = _service.CustomerService.GetAllCustomers(trackChanges: false);
            return Ok(customers);
        } catch
        {
            return StatusCode(500, "Internal server error");
        }
    }
}