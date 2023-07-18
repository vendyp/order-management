using FluentValidation;

namespace OrderManagementApi.WebApi.Endpoints.Identity;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(e => e.RefreshToken).NotNull().NotEmpty();
    }
}