using OrderManagementApi.Domain.Entities;
using Shouldly;

namespace OrderManagementApi.UnitTests.Entities;

public class OrderItemTests
{
    [Fact]
    public void OrderItem_CalculateTotalPrice_Should_Do_As_Expected()
    {
        const int quantity = 3;
        const decimal price = 15000m;
        const decimal totalPrice = price * 3;

        var ctor = new OrderItem
        {
            Quantity = quantity,
            Price = price
        };

        ctor.CalculateTotalPrice();
        ctor.TotalPrice.ShouldBe(totalPrice);
    }
}