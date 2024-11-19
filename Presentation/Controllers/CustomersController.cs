using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
namespace Presentation.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IServiceManager _service;
    public CustomersController(IServiceManager service) => _service = service;

    [HttpPost]
    public IActionResult CreateCustomer([FromBody] CreateCustomerRequestDto customer)
    {
        if (customer is null)
            return BadRequest("Request body can not be empty");

        var createdCustomer = _service.CustomerService.CreateCustomer(customer);

        return CreatedAtRoute("CustomerById", new { id = createdCustomer.Id }, createdCustomer);
    }

    [HttpGet]
    public IActionResult GetCustomers()
    {
        var customers = _service.CustomerService.GetAllCustomers(trackChanges: false);
            return Ok(customers);
    }

    [HttpGet("{id:guid}", Name = "CustomerById")]
    public IActionResult GetCustomer(Guid id)
    {
        var customer = _service.CustomerService.GetCustomer(id, trackChanges: false);
        return Ok(customer);
    }
}