using OrderManagementApi.Core.Modules;
using OrderManagementApi.WebApi.Scopes;
using OrderManagementApi.WebApi.Validators;
using FluentValidation;

namespace OrderManagementApi.WebApi.Endpoints.RoleManagement;

public class EditRoleRequestPayloadValidator : AbstractValidator<EditRoleRequestPayload>
{
    public EditRoleRequestPayloadValidator()
    {
        RuleFor(e => e.Scopes).NotNull();
        When(e => !string.IsNullOrWhiteSpace(e.Description),
            () => { RuleFor(e => e.Description).SetValidator(new NonUnicodeOnlyValidator()!).MaximumLength(512); });
        When(e => e.Scopes.Any(),
            () =>
            {
                RuleFor(e => e.Scopes).Must(e => e.Count == e.Distinct().Count());

                RuleForEach(e => e.Scopes)
                    .Must(e => ScopeManager.Instance.GetAllScopes().Any(f => f == e));
            });

        When(e => e.Modules.Any(), () =>
        {
            RuleFor(e => e.Modules).Must(e => e.Count == e.Distinct().Count());

            var list = new ModuleManager().GetAllModuleName();

            RuleForEach(e => e.Modules)
                .Must(e => list.Any(f => f == e));
        });
    }
}