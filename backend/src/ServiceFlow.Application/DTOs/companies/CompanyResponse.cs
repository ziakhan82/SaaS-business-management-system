namespace ServiceFlow.Application.DTOs.Companies;

public sealed record CompanyResponse(
    Guid Id,
    string Name,
    string Email,
    string? Phone,
    DateTime CreatedAtUtc
);