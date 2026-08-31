using Microsoft.AspNetCore.Mvc;
using ServiceFlow.Application.DTOs.Companies;
using ServiceFlow.Application.Interfaces.Services;

namespace ServiceFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpPost]
    public async Task<ActionResult<CompanyResponse>> Create(
        CreateCompanyRequest request,
        CancellationToken cancellationToken)
    {
        var company = await _companyService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = company.Id },
            company);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CompanyResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var company = await _companyService.GetByIdAsync(
            id,
            cancellationToken);

        if (company is null)
        {
            return NotFound();
        }

        return Ok(company);
    }
}