using OrderManagementApi.Domain.Enums;
using OrderManagementApi.Shared.Abstractions.Entities;

namespace OrderManagementApi.Domain.Entities;

public class Order : BaseEntity
{
    public Order()
    {
        OrderItems = new HashSet<OrderItem>();
    }

    public Guid OrderId { get; set; } = Guid.NewGuid();
    public string Number { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Requested;
    public ICollection<OrderItem> OrderItems { get; set; }

    public void RecalculateTotalPrice()
    {
        TotalPrice = 0;
        if (!OrderItems.Any())
            return;

        TotalPrice = OrderItems.Sum(e => e.TotalPrice);
    }
}