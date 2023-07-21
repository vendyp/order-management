using OrderManagementApi.WebApi.Shared.Abstractions;

namespace OrderManagementApi.WebApi.Endpoints.UserManagement.Scopes;

public class UserManagementScopeReadOnly : IScope
{
    public string ScopeName => $"{nameof(UserManagementScope)}.readonly".ToLower();
    public bool ExcludeResult => true;
}