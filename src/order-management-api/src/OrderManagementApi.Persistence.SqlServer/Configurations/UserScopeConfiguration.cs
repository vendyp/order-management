using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Entities;

namespace OrderManagementApi.Persistence.SqlServer.Configurations;

public class UserScopeConfiguration : BaseEntityConfiguration<UserScope>
{
    protected override void EntityConfiguration(EntityTypeBuilder<UserScope> builder)
    {
        builder.HasKey(e => e.UserScopeId);
        builder.Property(e => e.UserScopeId).ValueGeneratedNever();
        builder.Property(e => e.ScopeId).HasMaxLength(256);
    }
}