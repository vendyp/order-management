using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderManagementApi.Persistence.SqlServer.Configurations;

public class FileRepositoryConfiguration : BaseEntityConfiguration<FileRepository>
{
    protected override void EntityConfiguration(EntityTypeBuilder<FileRepository> builder)
    {
        builder.HasKey(e => e.FileRepositoryId);
        builder.Property(e => e.FileRepositoryId).ValueGeneratedNever();

        builder.Property(e => e.FileName).HasMaxLength(256);

        builder.Property(e => e.UniqueFileName).HasMaxLength(256);

        builder.Property(e => e.FileExtension).HasMaxLength(256);

        builder.Property(e => e.Note).HasMaxLength(512);

        builder.HasIndex(e => e.UniqueFileName).IsUnique();

        builder.Property(e => e.FilePath).HasMaxLength(2048);

        builder.Property(e => e.For).HasMaxLength(256);

        builder.HasIndex(e => e.For);
    }
}