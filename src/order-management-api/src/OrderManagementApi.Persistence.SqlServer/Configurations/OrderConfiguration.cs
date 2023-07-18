using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Entities;

namespace OrderManagementApi.Persistence.SqlServer.Configurations;

public class OrderConfiguration : BaseEntityConfiguration<Order>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.OrderId);
        builder.Property(e => e.OrderId).ValueGeneratedNever();
        builder.Property(e => e.Number).HasMaxLength(512);
        builder.Property(e => e.TotalPrice).HasPrecision(18, 2);
    }
}