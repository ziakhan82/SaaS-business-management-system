using ServiceFlow.Application.DTOs.Customers;
using ServiceFlow.Application.Interfaces.Repositories;
using ServiceFlow.Application.Interfaces.Services;
using ServiceFlow.Domain.Entities;

namespace ServiceFlow.Application.Services;

public sealed class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICompanyRepository _companyRepository;

    public CustomerService(
        ICustomerRepository customerRepository,
        ICompanyRepository companyRepository)
    {
        _customerRepository = customerRepository;
        _companyRepository = companyRepository;
    }

    public async Task<IReadOnlyList<CustomerResponse>> GetAllAsync(
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        var customers =
            await _customerRepository.GetAllByCompanyIdAsync(
                companyId,
                cancellationToken);

        return customers
            .Select(Map)
            .ToList();
    }

    public async Task<CustomerResponse?> GetByIdAsync(
        Guid id,
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await _customerRepository.GetByIdAsync(
                id,
                companyId,
                cancellationToken);

        return customer is null
            ? null
            : Map(customer);
    }

    public async Task<CustomerResponse?> CreateAsync(
        CreateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        var company =
            await _companyRepository.GetByIdAsync(
                request.CompanyId,
                cancellationToken);

        if (company is null)
        {
            return null;
        }

        var customer = new Customer
        {
            CompanyId = request.CompanyId,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = request.Email.Trim(),
            Phone = request.Phone?.Trim(),
            Address = request.Address?.Trim(),
            PostalCode = request.PostalCode?.Trim(),
            City = request.City?.Trim()
        };

        await _customerRepository.AddAsync(
            customer,
            cancellationToken);

        await _customerRepository.SaveChangesAsync(
            cancellationToken);

        return Map(customer);
    }

    public async Task<CustomerResponse?> UpdateAsync(
        Guid id,
        Guid companyId,
        UpdateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await _customerRepository.GetByIdAsync(
                id,
                companyId,
                cancellationToken);

        if (customer is null)
        {
            return null;
        }

        customer.FirstName = request.FirstName.Trim();
        customer.LastName = request.LastName.Trim();
        customer.Email = request.Email.Trim();
        customer.Phone = request.Phone?.Trim();
        customer.Address = request.Address?.Trim();
        customer.PostalCode = request.PostalCode?.Trim();
        customer.City = request.City?.Trim();

        await _customerRepository.SaveChangesAsync(
            cancellationToken);

        return Map(customer);
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await _customerRepository.GetByIdAsync(
                id,
                companyId,
                cancellationToken);

        if (customer is null)
        {
            return false;
        }

        _customerRepository.Delete(customer);

        await _customerRepository.SaveChangesAsync(
            cancellationToken);

        return true;
    }

    private static CustomerResponse Map(Customer customer)
    {
        return new CustomerResponse(
            customer.Id,
            customer.CompanyId,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.Phone,
            customer.Address,
            customer.PostalCode,
            customer.City,
            customer.CreatedAtUtc);
    }
}