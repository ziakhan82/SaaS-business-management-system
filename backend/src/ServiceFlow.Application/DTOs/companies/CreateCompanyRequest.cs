namespace ServiceFlow.Application.DTOs.Companies;

public sealed record CreateCompanyRequest(
    string Name,
    string Email,
    string? Phone
);