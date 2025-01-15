using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
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
    [HttpHead]
    public async Task<IActionResult> GetCustomers([FromQuery] CustomerParameters customerParameters)
    {
        var (customers, metaData) = await _service.CustomerService.GetAllCustomers(customerParameters, trackChanges: false);
        var paginationHeader = JsonSerializer.Serialize(metaData);
        Response.Headers.Add("X-Pagination", paginationHeader);
        return Ok(new
        {
            Customers = customers,
            MetaData = metaData
        });
    }

    [HttpGet("{id:guid}", Name = "CustomerById")]
    public async Task<IActionResult> GetCustomer(Guid id)
    {
        var customer = await _service.CustomerService.GetCustomer(id, trackChanges: false);
        return Ok(customer);
    }

    [HttpOptions]
    public IActionResult GetCustomersOptions()
    {
        Response.Headers.Add("Allow", "GET, OPTIONS, POST");
        return Ok();
    }
}