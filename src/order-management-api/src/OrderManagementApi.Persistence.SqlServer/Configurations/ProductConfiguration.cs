using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Entities;

namespace OrderManagementApi.Persistence.SqlServer.Configurations;

public class ProductConfiguration : BaseEntityConfiguration<Product>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.ProductId);
        builder.Property(e => e.ProductId).ValueGeneratedNever();
        builder.Property(e => e.Name).HasMaxLength(256);
        builder.Property(e => e.Image).HasMaxLength(256);
        builder.Property(e => e.Price).HasPrecision(18, 2);
    }
}