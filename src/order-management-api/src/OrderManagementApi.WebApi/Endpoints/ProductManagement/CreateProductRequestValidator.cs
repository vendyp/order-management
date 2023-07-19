using FluentValidation;
using OrderManagementApi.WebApi.Validators;

namespace OrderManagementApi.WebApi.Endpoints.ProductManagement;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty()
            .MaximumLength(300)
            .SetValidator(new NonUnicodeOnlyValidator());

        RuleFor(e => e.Description)
            .MaximumLength(1000)
            .SetValidator(new NonUnicodeOnlyValidator());

        RuleFor(e => e.File)
            .NotNull()
            .NotEmpty()
            .SetValidator(new IsGuidValidator()!);

        RuleFor(e => e.Price)
            .NotNull()
            .GreaterThan(0);
    }
}