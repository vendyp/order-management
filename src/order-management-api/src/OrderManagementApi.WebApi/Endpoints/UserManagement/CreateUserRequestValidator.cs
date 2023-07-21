using FluentValidation;
using OrderManagementApi.WebApi.Shared.Validators;

namespace OrderManagementApi.WebApi.Endpoints.UserManagement;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(e => e.Username).NotNull().NotEmpty().MaximumLength(256).SetValidator(new NonUnicodeOnlyValidator());
        RuleFor(e => e.Password).NotNull().NotEmpty().MaximumLength(256);
        RuleFor(e => e.Fullname).NotNull().NotEmpty().MaximumLength(256).SetValidator(new NonUnicodeOnlyValidator());
        RuleFor(e => e.EmailAddress).NotEmpty().MaximumLength(256).EmailAddress();
    }
}