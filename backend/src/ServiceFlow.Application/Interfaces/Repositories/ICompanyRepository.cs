using ServiceFlow.Domain.Entities;

namespace ServiceFlow.Application.Interfaces.Repositories;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Company company,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}