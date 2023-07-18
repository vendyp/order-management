using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Entities;

namespace OrderManagementApi.Persistence.SqlServer.Configurations;

public class OrderItemConfiguration : BaseEntityConfiguration<OrderItem>
{
    protected override void EntityConfiguration(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(e => e.OrderItemId);
        builder.Property(e => e.OrderItemId).ValueGeneratedNever();
        builder.Property(e => e.Price).HasPrecision(18, 2);
        builder.Property(e => e.TotalPrice).HasPrecision(18, 2);
    }
}