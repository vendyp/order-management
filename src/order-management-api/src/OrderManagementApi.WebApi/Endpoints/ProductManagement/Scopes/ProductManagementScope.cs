using OrderManagementApi.WebApi.Shared.Abstractions;

namespace OrderManagementApi.WebApi.Endpoints.ProductManagement.Scopes;

public class ProductManagementScope : IScope
{
    public string ScopeName => nameof(ProductManagementScope).ToLower();
    public bool ExcludeResult => true;
}