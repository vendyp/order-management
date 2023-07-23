using OrderManagementApi.WebApi.Shared.Abstractions;

namespace OrderManagementApi.WebApi.Endpoints.OrderManagement.Scopes;

public class OrderManagementScope : IScope
{
    public string ScopeName => nameof(OrderManagementScope).ToLower();
    public bool ExcludeResult => false;
}