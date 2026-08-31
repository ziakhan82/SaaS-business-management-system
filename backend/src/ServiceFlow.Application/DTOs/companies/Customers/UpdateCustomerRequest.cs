namespace ServiceFlow.Application.DTOs.Customers;

public sealed record UpdateCustomerRequest(
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? Address,
    string? PostalCode,
    string? City
);