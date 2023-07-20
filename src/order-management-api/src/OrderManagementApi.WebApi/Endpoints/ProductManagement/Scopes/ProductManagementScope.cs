using OrderManagementApi.WebApi.Scopes;

namespace OrderManagementApi.WebApi.Endpoints.ProductManagement.Scopes;

public class ProductManagementScope : IScope
{
    public string ScopeName => nameof(ProductManagementScope).ToLower();
}