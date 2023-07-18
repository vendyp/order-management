using OrderManagementApi.Core.Modules;
using OrderManagementApi.WebApi.Scopes;
using FluentValidation;

namespace OrderManagementApi.WebApi.Endpoints.RoleManagement;

public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
    public CreateRoleRequestValidator()
    {
        RuleFor(e => e.Name).NotNull().NotEmpty().MaximumLength(256);
        When(e => e.Scopes.Any(), () =>
        {
            RuleFor(e => e.Scopes).Must(e => e.Count == e.Distinct().Count());

            var list = ScopeManager.Instance.GetAllScopes();

            RuleForEach(e => e.Scopes).Must(e => list.Contains(e));
        });

        When(e => e.Modules.Any(), () =>
        {
            RuleFor(e => e.Modules).Must(e => e.Count == e.Distinct().Count());

            var list = new ModuleManager().GetAllModuleName();

            RuleForEach(e => e.Modules).Must(e => list.Contains(e));
        });
    }
}