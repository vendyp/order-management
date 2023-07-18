using OrderManagementApi.WebApi.Scopes;

namespace OrderManagementApi.WebApi.Endpoints.RoleManagement.Scopes;

public class RoleManagementScopeReadOnly : IScope
{
    public string ScopeName => $"{nameof(RoleManagementScope)}.readonly".ToLower();
}