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
        var customers = _service.CustomerService.GetAllCustomers(trackChanges: false);
            return Ok(customers);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetCustomer(Guid id)
    {
        var customer = _service.CustomerService.GetCustomer(id, trackChanges: false);
        return Ok(customer);
    }
}