using OrderManagementApi.WebApi.Shared.Abstractions;

namespace OrderManagementApi.WebApi.Shared.Scopes;

/// <summary>
/// Scope that shared for all user
/// </summary>
public class UserScope : IScope
{
    public string ScopeName => nameof(UserScope).ToLower();
    public bool ExcludeResult => false;
}