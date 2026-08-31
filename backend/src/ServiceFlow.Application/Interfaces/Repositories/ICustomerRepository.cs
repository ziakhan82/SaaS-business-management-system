using ServiceFlow.Domain.Entities;

namespace ServiceFlow.Application.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<IReadOnlyList<Customer>> GetAllByCompanyIdAsync(
        Guid companyId,
        CancellationToken cancellationToken = default);

    Task<Customer?> GetByIdAsync(
        Guid id,
        Guid companyId,
        CancellationToken cancellationToken = default);

    Task<bool> EmailExistsAsync(
    Guid companyId,
    string email,
    Guid? excludeCustomerId = null,
    CancellationToken cancellationToken = default);

    Task AddAsync(
        Customer customer,
        CancellationToken cancellationToken = default);

    void Delete(Customer customer);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}