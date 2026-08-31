namespace ServiceFlow.Application.DTOs.Customers;

public sealed record CreateCustomerRequest(
    Guid CompanyId,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? Address,
    string? PostalCode,
    string? City
);