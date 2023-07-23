using Microsoft.AspNetCore.Mvc;

namespace OrderManagementApi.WebApi.Endpoints.ProductManagement;

public class UpdateProductRequest
{
    [FromRoute(Name = "productId")] public Guid ProductId { get; set; }
    [FromBody] public UpdateProductRequestPayload Payload { get; set; } = null!;
}

public class UpdateProductRequestPayload
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? File { get; set; }
    public decimal? Price { get; set; }
}