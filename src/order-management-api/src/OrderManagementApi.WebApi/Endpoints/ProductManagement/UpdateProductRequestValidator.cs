using FluentValidation;
using OrderManagementApi.WebApi.Shared.Validators;

namespace OrderManagementApi.WebApi.Endpoints.ProductManagement;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequestPayload>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty()
            .MaximumLength(300)
            .SetValidator(new NonUnicodeOnlyValidator());

        RuleFor(e => e.Description)
            .MaximumLength(1000)
            .SetValidator(new NonUnicodeOnlyValidator());

        RuleFor(e => e.File)
            .SetValidator(new IsGuidValidator()!);

        RuleFor(e => e.Price)
            .NotNull()
            .GreaterThan(0);
    }
}