using ServiceFlow.Application.DTOs.Customers;

namespace ServiceFlow.Application.Interfaces.Services;

public interface ICustomerService
{
    Task<IReadOnlyList<CustomerResponse>> GetAllAsync(
        Guid companyId,
        CancellationToken cancellationToken = default);

    Task<CustomerResponse> GetByIdAsync(
        Guid id,
        Guid companyId,
        CancellationToken cancellationToken = default);

    Task<CustomerResponse> CreateAsync(
        CreateCustomerRequest request,
        CancellationToken cancellationToken = default);

    Task<CustomerResponse> UpdateAsync(
        Guid id,
        Guid companyId,
        UpdateCustomerRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        Guid companyId,
        CancellationToken cancellationToken = default);
}
