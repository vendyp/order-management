using FluentValidation;

namespace OrderManagementApi.WebApi.Endpoints.UserManagement;

public class UpdateUserRequestPayloadValidator : AbstractValidator<UpdateUserRequestPayload>
{
    public UpdateUserRequestPayloadValidator()
    {
        RuleFor(e => e.Fullname).NotNull().NotEmpty();
    }
}