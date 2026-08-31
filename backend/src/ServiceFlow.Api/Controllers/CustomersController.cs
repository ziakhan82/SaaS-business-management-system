using Microsoft.AspNetCore.Mvc;
using ServiceFlow.Application.DTOs.Customers;
using ServiceFlow.Application.Interfaces.Services;

namespace ServiceFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<
        ActionResult<IReadOnlyList<CustomerResponse>>> GetAll(
        [FromQuery] Guid companyId,
        CancellationToken cancellationToken)
    {
        var customers = await _customerService.GetAllAsync(
            companyId,
            cancellationToken);

        return Ok(customers);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerResponse>> GetById(
        Guid id,
        [FromQuery] Guid companyId,
        CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetByIdAsync(
            id,
            companyId,
            cancellationToken);

        if (customer is null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create(
        CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerService.CreateAsync(
            request,
            cancellationToken);

        if (customer is null)
        {
            return BadRequest(new
            {
                message = "The specified company does not exist."
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new
            {
                id = customer.Id,
                companyId = customer.CompanyId
            },
            customer);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CustomerResponse>> Update(
        Guid id,
        [FromQuery] Guid companyId,
        UpdateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerService.UpdateAsync(
            id,
            companyId,
            request,
            cancellationToken);

        if (customer is null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromQuery] Guid companyId,
        CancellationToken cancellationToken)
    {
        var deleted = await _customerService.DeleteAsync(
            id,
            companyId,
            cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}