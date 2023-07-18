using OrderManagementApi.Domain.Entities;
using Shouldly;

namespace OrderManagementApi.UnitTests.Entities;

public class OrderTests
{
    [Fact]
    public void Order_RecalculateTotalPrice_Should_Do_As_Expected()
    {
        const int qty1 = 3;
        const decimal price1 = 7500m;
        var orderItem1 = new OrderItem
        {
            Quantity = qty1,
            Price = price1
        };
        orderItem1.CalculateTotalPrice();
        const int qty2 = 4;
        const decimal price2 = 6500m;
        var orderItem2 = new OrderItem
        {
            Quantity = qty2,
            Price = price2
        };
        orderItem2.CalculateTotalPrice();
        const int qty3 = 7;
        const decimal price3 = 8000m;
        var orderItem3 = new OrderItem
        {
            Quantity = qty3,
            Price = price3
        };
        orderItem3.CalculateTotalPrice();

        decimal expectedTotalPrice = orderItem1.TotalPrice + orderItem2.TotalPrice + orderItem3.TotalPrice;

        var order = new Order();
        order.OrderItems.Add(orderItem1);
        order.OrderItems.Add(orderItem2);
        order.OrderItems.Add(orderItem3);
        order.RecalculateTotalPrice();

        order.TotalPrice.ShouldBe(expectedTotalPrice);
    }
}