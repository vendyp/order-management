using OrderManagementApi.WebApi.Scopes;

namespace OrderManagementApi.WebApi.Endpoints.UserManagement.Scopes;

public class UserManagementScope : IScope
{
    public string ScopeName => nameof(UserManagementScope).ToLower();
}