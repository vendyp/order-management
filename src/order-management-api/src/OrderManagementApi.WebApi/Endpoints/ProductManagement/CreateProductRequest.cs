namespace OrderManagementApi.WebApi.Endpoints.ProductManagement;

public class CreateProductRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? File { get; set; }
    public decimal? Price { get; set; }
}