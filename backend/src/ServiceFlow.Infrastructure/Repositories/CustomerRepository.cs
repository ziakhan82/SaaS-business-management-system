using Microsoft.EntityFrameworkCore;
using ServiceFlow.Application.Interfaces.Repositories;
using ServiceFlow.Domain.Entities;
using ServiceFlow.Infrastructure.Persistence;

namespace ServiceFlow.Infrastructure.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _dbContext;

    public CustomerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Customer>>
        GetAllByCompanyIdAsync(
            Guid companyId,
            CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .Where(customer =>
                customer.CompanyId == companyId)
            .OrderBy(customer => customer.FirstName)
            .ThenBy(customer => customer.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Customer?> GetByIdAsync(
        Guid id,
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers
            .FirstOrDefaultAsync(
                customer =>
                    customer.Id == id &&
                    customer.CompanyId == companyId,
                cancellationToken);
    }

    public async Task AddAsync(
        Customer customer,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Customers.AddAsync(
            customer,
            cancellationToken);
    }

    public void Delete(Customer customer)
    {
        _dbContext.Customers.Remove(customer);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }
}