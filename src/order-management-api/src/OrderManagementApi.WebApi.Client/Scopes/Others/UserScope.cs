using OrderManagementApi.Core.Abstractions;

namespace OrderManagementApi.WebApi.Client.Scopes.Others;

public class UserScope : IScope
{
    public string ScopeName => nameof(UserScope).ToLower();
    public bool ExcludeResult => false;
}