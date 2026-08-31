using Microsoft.EntityFrameworkCore;
using ServiceFlow.Application.Interfaces.Repositories;
using ServiceFlow.Domain.Entities;
using ServiceFlow.Infrastructure.Persistence;

namespace ServiceFlow.Infrastructure.Repositories;

public sealed class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _dbContext;

    public CompanyRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Company?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Companies
            .FirstOrDefaultAsync(
                company => company.Id == id,
                cancellationToken);
    }

    public async Task AddAsync(
        Company company,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Companies.AddAsync(
            company,
            cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }
}