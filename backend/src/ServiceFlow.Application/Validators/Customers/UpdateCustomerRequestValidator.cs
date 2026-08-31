using FluentValidation;
using ServiceFlow.Application.DTOs.Customers;

namespace ServiceFlow.Application.Validators.Customers;

public sealed class UpdateCustomerRequestValidator
    : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(request => request.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(100);

        RuleFor(request => request.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(100);

        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("A valid email address is required.")
            .MaximumLength(320);

        RuleFor(request => request.Phone)
            .MaximumLength(50);

        RuleFor(request => request.Address)
            .MaximumLength(250);

        RuleFor(request => request.PostalCode)
            .MaximumLength(20);

        RuleFor(request => request.City)
            .MaximumLength(100);
    }
}