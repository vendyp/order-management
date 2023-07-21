using FluentValidation;
using OrderManagementApi.WebApi.Shared.Validators;

namespace OrderManagementApi.WebApi.Endpoints.Identity;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(e => e.Username).NotNull().NotEmpty().MaximumLength(100).MinimumLength(3)
            .SetValidator(new NonUnicodeOnlyValidator());
        RuleFor(e => e.Password).NotNull().NotEmpty().MaximumLength(256);
    }
}