using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderManagementApi.Persistence.SqlServer.Configurations;

public class RoleModuleConfiguration : BaseEntityConfiguration<RoleModule>
{
    protected override void EntityConfiguration(EntityTypeBuilder<RoleModule> builder)
    {
        builder.HasKey(e => e.RoleModuleId);
        builder.Property(e => e.RoleModuleId).ValueGeneratedNever();

        builder.Property(e => e.RoleId).HasMaxLength(100);

        builder.Property(e => e.Name).HasMaxLength(256);
    }
}