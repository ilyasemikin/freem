using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Configurations;

internal sealed class RunningRecordActivityRelationEntityTypeConfiguration : IEntityTypeConfiguration<RunningRecordActivityRelationEntity>
{
    public void Configure(EntityTypeBuilder<RunningRecordActivityRelationEntity> builder)
    {
        builder.ToTable(RelationsNames.RunningRecordsActivities.Table);

        builder
            .Property(e => e.RunningRecordUserId)
            .HasColumnName(RelationsNames.RunningRecordsActivities.Properties.RunningRecordId)
            .IsRequired();

        builder
            .Property(e => e.ActivityId)
            .HasColumnName(RelationsNames.RunningRecordsActivities.Properties.ActivityId)
            .IsRequired();

        builder
            .HasKey(e => new { e.RunningRecordUserId, e.ActivityId })
            .HasName(RelationsNames.RunningRecordsActivities.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.ActivityId)
            .HasDatabaseName(RelationsNames.RunningRecordsActivities.Constraints.ActivityIdIndex);

        builder
            .HasOne(e => e.RunningRecord)
            .WithMany()
            .HasForeignKey(e => e.RunningRecordUserId)
            .HasConstraintName(RelationsNames.RunningRecordsActivities.Constraints.RunningRecordsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne(e => e.Activity)
            .WithMany()
            .HasForeignKey(e => e.ActivityId)
            .HasConstraintName(RelationsNames.RunningRecordsActivities.Constraints.ActivitiesForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
