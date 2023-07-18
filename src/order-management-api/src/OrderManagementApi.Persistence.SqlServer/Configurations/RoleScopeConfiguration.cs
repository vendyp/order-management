using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderManagementApi.Persistence.SqlServer.Configurations;

public class RoleScopeConfiguration : BaseEntityConfiguration<RoleScope>
{
    protected override void EntityConfiguration(EntityTypeBuilder<RoleScope> builder)
    {
        builder.HasKey(e => e.RoleScopeId);
        builder.Property(e => e.RoleScopeId).ValueGeneratedNever();

        builder.Property(e => e.RoleId).HasMaxLength(100);

        builder.Property(e => e.Name).HasMaxLength(256);
    }
}