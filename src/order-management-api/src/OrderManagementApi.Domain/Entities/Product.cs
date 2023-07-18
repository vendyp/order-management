using OrderManagementApi.Shared.Abstractions.Entities;

namespace OrderManagementApi.Domain.Entities;

public class Product : BaseEntity
{
    public Guid ProductId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Image { get; set; }
    public decimal Price { get; set; }
}