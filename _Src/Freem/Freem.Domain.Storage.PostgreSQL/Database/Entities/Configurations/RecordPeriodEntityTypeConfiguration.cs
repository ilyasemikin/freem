using Freem.Domain.Records.Models;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class RecordPeriodEntityTypeConfiguration : IEntityTypeConfiguration<RecordPeriodEntity>
{
    public void Configure(EntityTypeBuilder<RecordPeriodEntity> builder)
    {
        builder.ToTable(EntitiesNames.RecordPeriods.Table, table =>
            table.HasCheckConstraint(
                EntitiesNames.RecordPeriods.Constraints.TimePeriodCheck,
                $"{EntitiesNames.RecordPeriods.Properties.StartAt} < {EntitiesNames.RecordPeriods.Properties.EndAt}"));

        builder
            .Property(e => e.Id)
            .HasColumnName(EntitiesNames.RecordPeriods.Properties.Id)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.RecordId)
            .HasColumnName(EntitiesNames.RecordPeriods.Properties.RecordId)
            .IsRequired();

        builder
            .Property(e => e.Description)
            .HasMaxLength(RecordPeriodDescription.MaxLength)
            .HasColumnName(EntitiesNames.RecordPeriods.Properties.Description);

        builder
            .Property(e => e.StartAt)
            .HasColumnName(EntitiesNames.RecordPeriods.Properties.StartAt)
            .IsRequired();

        builder
            .Property(e => e.EndAt)
            .HasColumnName(EntitiesNames.RecordPeriods.Properties.EndAt)
            .IsRequired();

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(EntitiesNames.RecordPeriods.Properties.CreatedAt)
            .IsRequired();

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName(EntitiesNames.RecordPeriods.Properties.UpdatedAt);

        builder
            .HasKey(e => e.Id)
            .HasName(EntitiesNames.RecordPeriods.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.RecordId)
            .HasDatabaseName(EntitiesNames.RecordPeriods.Constraints.RecordIdIndex);

        builder
            .HasIndex(e => new { e.StartAt, e.EndAt })
            .HasDatabaseName(EntitiesNames.RecordPeriods.Constraints.TimePeriodIndex);

        builder
            .HasOne(e => e.Record)
            .WithMany(e => e.Periods)
            .HasForeignKey(e => e.RecordId)
            .HasConstraintName(EntitiesNames.RecordPeriods.Constraints.RecordsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
