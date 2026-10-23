using Freem.Domain.Formats;
using Freem.Domain.Import;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class ImportDocumentEntityTypeConfiguration : IEntityTypeConfiguration<ImportDocumentEntity>
{
    public void Configure(EntityTypeBuilder<ImportDocumentEntity> builder)
    {
        builder.ToTable(EntitiesNames.ImportDocuments.Table, table =>
            table.HasCheckConstraint(
                EntitiesNames.ImportDocuments.Constraints.StatusCheck,
                $"{EntitiesNames.ImportDocuments.Properties.Status} IN ('Created', 'Parsing', 'Parsed', 'Failed')"));

        builder
            .Property(e => e.Id)
            .HasColumnName(EntitiesNames.ImportDocuments.Properties.Id)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.UserId)
            .HasColumnName(EntitiesNames.ImportDocuments.Properties.UserId)
            .IsRequired();

        builder
            .Property(e => e.Format)
            .HasColumnName(EntitiesNames.ImportDocuments.Properties.Format)
            .HasMaxLength(FormatKey.MaxLength)
            .IsRequired();

        builder
            .Property(e => e.FileName)
            .HasColumnName(EntitiesNames.ImportDocuments.Properties.FileName)
            .HasMaxLength(ImportFileName.MaxLength)
            .IsRequired();

        builder
            .Property(e => e.TimeZoneOffsetTicks)
            .HasColumnName(EntitiesNames.ImportDocuments.Properties.TimeZoneOffsetTicks)
            .IsRequired();

        builder
            .Property(e => e.Status)
            .HasColumnName(EntitiesNames.ImportDocuments.Properties.Status)
            .HasMaxLength(32)
            .IsRequired();

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(EntitiesNames.ImportDocuments.Properties.CreatedAt).IsRequired();

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName(EntitiesNames.ImportDocuments.Properties.UpdatedAt);

        builder
            .HasKey(e => e.Id)
            .HasName(EntitiesNames.ImportDocuments.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.UserId)
            .HasDatabaseName(EntitiesNames.ImportDocuments.Constraints.UserIdIndex);

        builder
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .HasConstraintName(EntitiesNames.ImportDocuments.Constraints.UsersForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
