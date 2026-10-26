using Freem.Domain.DataTransfer.Formats;
using Freem.Domain.Records.Models;
using Freem.Domain.Records.Sources;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class RecordEntityTypeConfiguration : IEntityTypeConfiguration<RecordEntity>
{
    public void Configure(EntityTypeBuilder<RecordEntity> builder)
    {
        builder.ToTable(EntitiesNames.Records.Table, table =>
        {
            var type = EntitiesNames.Records.Properties.SourceType;
            var manual = RecordSourceType.Manual.ToString("G");
            var runningRecord = RecordSourceType.RunningRecord.ToString("G");
            var import = RecordSourceType.Import.ToString("G");
            string[] importColumns =
            [
                EntitiesNames.Records.Properties.SourceImportDocumentId,
                EntitiesNames.Records.Properties.SourceImportRowId,
                EntitiesNames.Records.Properties.SourceImportFormat,
                EntitiesNames.Records.Properties.SourceImportRecordId
            ];
            var importColumnsSet = string.Join(" AND ", importColumns.Select(c => $"{c} IS NOT NULL"));
            var importColumnsEmpty = string.Join(" AND ", importColumns.Select(c => $"{c} IS NULL"));

            table.HasCheckConstraint(
                EntitiesNames.Records.Constraints.SourceTypeCheck,
                $"{type} IN ('{manual}', '{runningRecord}', '{import}')");
            table.HasCheckConstraint(
                EntitiesNames.Records.Constraints.SourceImportCheck,
                $"({type} = '{import}' AND {importColumnsSet}) OR ({type} <> '{import}' AND {importColumnsEmpty})");
        });

        builder
            .Property(e => e.Id)
            .HasColumnName(EntitiesNames.Records.Properties.Id)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.UserId)
            .HasColumnName(EntitiesNames.Records.Properties.UserId)
            .IsRequired();

        builder
            .Property(e => e.Name)
            .HasMaxLength(RecordName.MaxLength)
            .HasColumnName(EntitiesNames.Records.Properties.Name);

        builder
            .Property(e => e.Description)
            .HasMaxLength(RecordDescription.MaxLength)
            .HasColumnName(EntitiesNames.Records.Properties.Description);

        builder
            .Property(e => e.SourceType)
            .HasColumnName(EntitiesNames.Records.Properties.SourceType)
            .IsRequired();

        builder
            .Property(e => e.SourceImportDocumentId)
            .HasColumnName(EntitiesNames.Records.Properties.SourceImportDocumentId);

        builder
            .Property(e => e.SourceImportRowId)
            .HasColumnName(EntitiesNames.Records.Properties.SourceImportRowId);

        builder
            .Property(e => e.SourceImportFormat)
            .HasColumnName(EntitiesNames.Records.Properties.SourceImportFormat)
            .HasMaxLength(FormatKey.MaxLength);

        builder
            .Property(e => e.SourceImportRecordId)
            .HasColumnName(EntitiesNames.Records.Properties.SourceImportRecordId);

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(EntitiesNames.Records.Properties.CreatedAt)
            .IsRequired();

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName(EntitiesNames.Records.Properties.UpdatedAt);

        builder
            .HasKey(e => e.Id)
            .HasName(EntitiesNames.Records.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.UserId)
            .HasDatabaseName(EntitiesNames.Records.Constraints.UserIdIndex);

        builder
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .HasConstraintName(EntitiesNames.Records.Constraints.UsersForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasMany(e => e.Activities)
            .WithMany()
            .UsingEntity<RecordActivityRelationEntity>();

        builder
            .HasMany(e => e.Tags)
            .WithMany(e => e.Records)
            .UsingEntity<RecordTagRelationEntity>();
    }
}
