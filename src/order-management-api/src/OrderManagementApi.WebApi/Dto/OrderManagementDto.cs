using OrderManagementApi.Domain.Enums;

namespace OrderManagementApi.WebApi.Dto;

public record OrderManagementDto
{
    public Guid OrderId { get; set; }
    public string? Number { get; set; }
    public decimal TotalPrice { get; set; }
    public int TotalItems { get; set; }
    public OrderStatus Status { get; set; }
}