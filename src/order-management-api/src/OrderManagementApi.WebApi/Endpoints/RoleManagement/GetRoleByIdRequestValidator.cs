using OrderManagementApi.Domain.Extensions;
using FluentValidation;

namespace OrderManagementApi.WebApi.Endpoints.RoleManagement;

public class GetRoleByIdRequestValidator : AbstractValidator<GetRoleByIdRequest>
{
    public GetRoleByIdRequestValidator()
    {
        RuleFor(e => e.RoleId).Must(e => e != RoleExtensions.SuperAdministratorId);
    }
}