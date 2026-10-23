using Freem.Domain.Records.Models;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class ImportRowPeriodEntityTypeConfiguration : IEntityTypeConfiguration<ImportRowPeriodEntity>
{
    public void Configure(EntityTypeBuilder<ImportRowPeriodEntity> builder)
    {
        builder.ToTable(EntitiesNames.ImportRowPeriods.Table, table =>
            table.HasCheckConstraint(
                EntitiesNames.ImportRowPeriods.Constraints.TimePeriodCheck,
                $"{EntitiesNames.ImportRowPeriods.Properties.StartAt} <= {EntitiesNames.ImportRowPeriods.Properties.EndAt}"));

        builder
            .Property(e => e.RowId)
            .HasColumnName(EntitiesNames.ImportRowPeriods.Properties.RowId)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.Position)
            .HasColumnName(EntitiesNames.ImportRowPeriods.Properties.Position)
            .IsRequired();

        builder
            .Property(e => e.Description)
            .HasColumnName(EntitiesNames.ImportRowPeriods.Properties.Description)
            .HasMaxLength(RecordPeriodDescription.MaxLength);

        builder
            .Property(e => e.StartAt)
            .HasColumnName(EntitiesNames.ImportRowPeriods.Properties.StartAt)
            .IsRequired();

        builder
            .Property(e => e.EndAt)
            .HasColumnName(EntitiesNames.ImportRowPeriods.Properties.EndAt)
            .IsRequired();

        builder
            .Property(e => e.TimeZoneOffsetTicks)
            .HasColumnName(EntitiesNames.ImportRowPeriods.Properties.TimeZoneOffsetTicks)
            .IsRequired();

        builder
            .HasKey(e => new { e.RowId, e.Position })
            .HasName(EntitiesNames.ImportRowPeriods.Constraints.PrimaryKey);

        builder
            .HasIndex(e => new { e.StartAt, e.EndAt })
            .HasDatabaseName(EntitiesNames.ImportRowPeriods.Constraints.TimePeriodIndex);

        builder
            .HasOne(e => e.Row)
            .WithMany(e => e.Periods)
            .HasForeignKey(e => e.RowId)
            .HasConstraintName(EntitiesNames.ImportRowPeriods.Constraints.RowsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
