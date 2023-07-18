using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderManagementApi.Persistence.SqlServer.Configurations;

public class RoleConfiguration : BaseEntityConfiguration<Role>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(e => e.RoleId);
        builder.Property(e => e.RoleId).ValueGeneratedNever().HasMaxLength(100);

        builder.Property(e => e.Name).HasMaxLength(100);

        builder.Property(e => e.Description).HasMaxLength(512);
    }
}