using OrderManagementApi.WebApi.Shared.Abstractions;

namespace OrderManagementApi.WebApi.Endpoints.UserManagement.Scopes;

public class UserManagementScope : IScope
{
    public string ScopeName => nameof(UserManagementScope).ToLower();
    public bool ExcludeResult => true;
}