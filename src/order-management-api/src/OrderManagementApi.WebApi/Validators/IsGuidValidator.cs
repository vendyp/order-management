using FluentValidation;

namespace OrderManagementApi.WebApi.Validators;

public class IsGuidValidator : AbstractValidator<string>
{
    public IsGuidValidator()
    {
        RuleFor(x => x)
            .Must(IsGuid)
            .WithMessage("The string must a valid guid.");
    }

    private static bool IsGuid(string value)
    {
        return Guid.TryParse(value, out _);
    }
}