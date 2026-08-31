namespace ServiceFlow.Application.DTOs.Customers;

public sealed record CustomerResponse(
    Guid Id,
    Guid CompanyId,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? Address,
    string? PostalCode,
    string? City,
    DateTime CreatedAtUtc
);