using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderManagementApi.Persistence.SqlServer.Configurations;

public class EmailSenderConfiguration : BaseEntityConfiguration<EmailSender>
{
    protected override void EntityConfiguration(EntityTypeBuilder<EmailSender> builder)
    {
        builder.HasKey(e => e.EmailSenderId);
        builder.Property(e => e.EmailSenderId).ValueGeneratedNever();
        builder.Property(e => e.HashedEmailSender)
            .HasMaxLength(1024);
        builder.HasIndex(e => e.HashedEmailSender).IsUnique();
        builder.Property(e => e.Subject)
            .HasMaxLength(512);
        builder.Property(e => e.Source)
            .HasMaxLength(512);
        builder.Property(e => e.Template)
            .HasMaxLength(256);
    }
}