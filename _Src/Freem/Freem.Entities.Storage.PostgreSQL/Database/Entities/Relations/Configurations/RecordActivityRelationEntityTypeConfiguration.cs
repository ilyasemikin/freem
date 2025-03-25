using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Configurations;

internal sealed class RecordActivityRelationEntityTypeConfiguration : IEntityTypeConfiguration<RecordActivityRelationEntity>
{
    public void Configure(EntityTypeBuilder<RecordActivityRelationEntity> builder)
    {
        builder.ToTable(RelationsNames.RecordsActivities.Table);

        builder
            .Property(e => e.RecordId)
            .HasColumnName(RelationsNames.RecordsActivities.Properties.RecordId)
            .IsRequired();

        builder
            .Property(e => e.ActivityId)
            .HasColumnName(RelationsNames.RecordsActivities.Properties.ActivityId)
            .IsRequired();

        builder
            .HasKey(e => new { e.RecordId, e.ActivityId })
            .HasName(RelationsNames.RecordsActivities.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.ActivityId)
            .HasDatabaseName(RelationsNames.RecordsActivities.Constraints.ActivityIdIndex);

        builder
            .HasOne(e => e.Record)
            .WithMany()
            .HasForeignKey(e => e.RecordId)
            .HasConstraintName(RelationsNames.RecordsActivities.Constraints.RecordsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne(e => e.Activity)
            .WithMany()
            .HasForeignKey(e => e.ActivityId)
            .HasConstraintName(RelationsNames.RecordsActivities.Constraints.ActivitiesForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
