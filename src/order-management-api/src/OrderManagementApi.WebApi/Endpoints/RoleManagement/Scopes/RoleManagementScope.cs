using OrderManagementApi.WebApi.Scopes;

namespace OrderManagementApi.WebApi.Endpoints.RoleManagement.Scopes;

public class RoleManagementScope : IScope
{
    public string ScopeName => nameof(RoleManagementScope).ToLower();
}