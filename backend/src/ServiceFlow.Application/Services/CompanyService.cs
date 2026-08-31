using FluentValidation;
using ServiceFlow.Application.DTOs.Companies;
using ServiceFlow.Application.Interfaces.Repositories;
using ServiceFlow.Application.Interfaces.Services;
using ServiceFlow.Domain.Entities;

namespace ServiceFlow.Application.Services;

public sealed class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IValidator<CreateCompanyRequest> _createValidator;

    public CompanyService(
        ICompanyRepository companyRepository,
        IValidator<CreateCompanyRequest> createValidator)
    {
        _companyRepository = companyRepository;
        _createValidator = createValidator;
    }

    public async Task<CompanyResponse> CreateAsync(
        CreateCompanyRequest request,
        CancellationToken cancellationToken = default)
    {
        await _createValidator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var company = new Company
        {
            Name = request.Name.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            Phone = request.Phone?.Trim()
        };

        await _companyRepository.AddAsync(
            company,
            cancellationToken);

        await _companyRepository.SaveChangesAsync(
            cancellationToken);

        return Map(company);
    }

    public async Task<CompanyResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var company = await _companyRepository.GetByIdAsync(
            id,
            cancellationToken);

        return company is null
            ? null
            : Map(company);
    }

    private static CompanyResponse Map(Company company)
    {
        return new CompanyResponse(
            company.Id,
            company.Name,
            company.Email,
            company.Phone,
            company.CreatedAtUtc);
    }
}
