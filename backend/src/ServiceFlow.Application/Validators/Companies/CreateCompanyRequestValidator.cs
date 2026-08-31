using FluentValidation;
using ServiceFlow.Application.DTOs.Companies;

namespace ServiceFlow.Application.Validators.Companies;

public sealed class CreateCompanyRequestValidator
    : AbstractValidator<CreateCompanyRequest>
{
    public CreateCompanyRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage("Company name is required.")
            .MaximumLength(200)
            .WithMessage("Company name cannot exceed 200 characters.");

        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("A valid email address is required.")
            .MaximumLength(320);

        RuleFor(request => request.Phone)
            .MaximumLength(50)
            .When(request => !string.IsNullOrWhiteSpace(request.Phone));
    }
}