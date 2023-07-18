using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Entities;

namespace OrderManagementApi.Persistence.SqlServer.Configurations;

public class CartConfiguration : BaseEntityConfiguration<Cart>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(e => e.CartId);
        builder.Property(e => e.CartId).ValueGeneratedNever();
    }
}