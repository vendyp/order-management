using OrderManagementApi.Domain.Entities;
using OrderManagementApi.WebApi.Scopes;

namespace OrderManagementApi.UnitTests.Builders.Entities;

public class RoleBuilder
{
    private Role? _role;

    public RoleBuilder Create(string id, string name)
    {
        _role = new Role(id, name);

        return this;
    }

    public RoleBuilder SetupScope(string name)
    {
        _role!.RoleScopes.Add(new RoleScope(_role.RoleId, name));

        return this;
    }

    public RoleBuilder SetupScopeAll()
    {
        foreach (var item in ScopeManager.Instance.GetAllScopes())
            _role!.RoleScopes.Add(new RoleScope(_role.RoleId, item));

        return this;
    }

    public Role Build()
    {
        return _role!;
    }
}