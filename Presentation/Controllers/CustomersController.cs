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
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequestDto customer)
    {
        if (customer is null)
            return BadRequest("Request body can not be empty");

        var createdCustomer = await _service.CustomerService.CreateCustomer(customer);

        return CreatedAtRoute("CustomerById", new { id = createdCustomer!.Id }, createdCustomer);
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await _service.CustomerService.GetAllCustomers(trackChanges: false);
            return Ok(customers);
    }

    [HttpGet("{id:guid}", Name = "CustomerById")]
    public async Task<IActionResult> GetCustomer(Guid id)
    {
        var customer = await _service.CustomerService.GetCustomer(id, trackChanges: false);
        return Ok(customer);
    }
}