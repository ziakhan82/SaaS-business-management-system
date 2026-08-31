using ServiceFlow.Application.DTOs.Companies;

namespace ServiceFlow.Application.Interfaces.Services;

public interface ICompanyService
{
    Task<CompanyResponse> CreateAsync(
        CreateCompanyRequest request,
        CancellationToken cancellationToken = default);

    Task<CompanyResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}