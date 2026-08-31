using FluentValidation;
using ServiceFlow.Application.Common.Exceptions;
using ServiceFlow.Application.DTOs.Customers;
using ServiceFlow.Application.Interfaces.Repositories;
using ServiceFlow.Application.Interfaces.Services;
using ServiceFlow.Domain.Entities;

namespace ServiceFlow.Application.Services;

public sealed class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IValidator<CreateCustomerRequest> _createValidator;
    private readonly IValidator<UpdateCustomerRequest> _updateValidator;

    public CustomerService(
        ICustomerRepository customerRepository,
        ICompanyRepository companyRepository,
        IValidator<CreateCustomerRequest> createValidator,
        IValidator<UpdateCustomerRequest> updateValidator)
    {
        _customerRepository = customerRepository;
        _companyRepository = companyRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
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

    public async Task<CustomerResponse> GetByIdAsync(
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
            throw new NotFoundException(
                "Customer",
                id);
        }

        return Map(customer);
    }

    public async Task<CustomerResponse> CreateAsync(
        CreateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        await _createValidator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var company = await _companyRepository.GetByIdAsync(
            request.CompanyId,
            cancellationToken);

        if (company is null)
        {
            throw new NotFoundException(
                "Company",
                request.CompanyId);
        }

        var normalizedEmail =
            request.Email.Trim().ToLowerInvariant();

        var emailExists =
            await _customerRepository.EmailExistsAsync(
                request.CompanyId,
                normalizedEmail,
                cancellationToken: cancellationToken);

        if (emailExists)
        {
            throw new ConflictException(
                "A customer with this email already exists.");
        }

        var customer = new Customer
        {
            CompanyId = request.CompanyId,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = normalizedEmail,
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

    public async Task<CustomerResponse> UpdateAsync(
        Guid id,
        Guid companyId,
        UpdateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        await _updateValidator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var customer =
            await _customerRepository.GetByIdAsync(
                id,
                companyId,
                cancellationToken);

        if (customer is null)
        {
            throw new NotFoundException(
                "Customer",
                id);
        }

        var normalizedEmail =
            request.Email.Trim().ToLowerInvariant();

        var emailExists =
            await _customerRepository.EmailExistsAsync(
                companyId,
                normalizedEmail,
                id,
                cancellationToken);

        if (emailExists)
        {
            throw new ConflictException(
                "Another customer with this email already exists.");
        }

        customer.FirstName = request.FirstName.Trim();
        customer.LastName = request.LastName.Trim();
        customer.Email = normalizedEmail;
        customer.Phone = request.Phone?.Trim();
        customer.Address = request.Address?.Trim();
        customer.PostalCode = request.PostalCode?.Trim();
        customer.City = request.City?.Trim();

        await _customerRepository.SaveChangesAsync(
            cancellationToken);

        return Map(customer);
    }

    public async Task DeleteAsync(
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
            throw new NotFoundException(
                "Customer",
                id);
        }

        _customerRepository.Delete(customer);

        await _customerRepository.SaveChangesAsync(
            cancellationToken);

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
