using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations.Events.Base;

internal sealed class BaseEventEntityConfiguration : IEntityTypeConfiguration<BaseEventEntity>
{
    public void Configure(EntityTypeBuilder<BaseEventEntity> builder)
    {
        builder
            .ToTable(EntitiesNames.Events.Table, table =>
            {
                table.HasCheckConstraint(
                    EntitiesNames.Events.Constraints.UserIdCheck,
                    $"{EntitiesNames.Events.Properties.UserId} is not null");

                table.HasCheckConstraint(
                    EntitiesNames.Events.Constraints.EventTypeCheck,
                    $"({EntitiesNames.Events.Properties.EventType} = '{EntitiesNames.Events.Activities.EventType}' and {EntitiesNames.Events.Activities.Properties.ActivityId} is not null and {EntitiesNames.Events.Records.Properties.RecordId} is null and {EntitiesNames.Events.Tags.Properties.TagId} is null) or " +
                    $"({EntitiesNames.Events.Properties.EventType} = '{EntitiesNames.Events.Records.EventType}' and {EntitiesNames.Events.Activities.Properties.ActivityId} is null and {EntitiesNames.Events.Records.Properties.RecordId} is not null and {EntitiesNames.Events.Tags.Properties.TagId} is null) or"  +
                    $"({EntitiesNames.Events.Properties.EventType} = '{EntitiesNames.Events.RunningRecords.EventType}' and {EntitiesNames.Events.Activities.Properties.ActivityId} is null and {EntitiesNames.Events.Records.Properties.RecordId} is null and {EntitiesNames.Events.Tags.Properties.TagId} is null) or " +
                    $"({EntitiesNames.Events.Properties.EventType} = '{EntitiesNames.Events.Tags.EventType}' and {EntitiesNames.Events.Activities.Properties.ActivityId} is null and {EntitiesNames.Events.Records.Properties.RecordId} is null and {EntitiesNames.Events.Tags.Properties.TagId} is not null)");
            });

        builder
            .HasDiscriminator(e => e.EventType)
            .HasValue<ActivityEventEntity>(EntitiesNames.Events.Activities.EventType)
            .HasValue<RecordEventEntity>(EntitiesNames.Events.Records.EventType)
            .HasValue<RunningRecordEventEntity>(EntitiesNames.Events.RunningRecords.EventType)
            .HasValue<TagEventEntity>(EntitiesNames.Events.Tags.EventType)
            .IsComplete();

        builder
            .Property(e => e.Id)
            .HasColumnName(EntitiesNames.Events.Properties.Id)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.UserId)
            .HasColumnName(EntitiesNames.Events.Properties.UserId)
            .HasColumnOrder(1)
            .IsRequired();

        builder
            .Property(e => e.Action)
            .HasColumnName(EntitiesNames.Events.Properties.Action)
            .HasColumnType($"{EnvironmentNames.Schema}.{EntitiesNames.Events.Models.Action}")
            .HasColumnOrder(3)
            .IsRequired();

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(EntitiesNames.Events.Properties.CreatedAt)
            .HasColumnOrder(2)
            .IsRequired();

        builder
            .Property(e => e.EventType)
            .HasColumnName(EntitiesNames.Events.Properties.EventType)
            .HasMaxLength(BaseEventEntity.EventTypeMaxLength)
            .IsRequired();
        
        builder
            .HasKey(e => e.Id)
            .HasName(EntitiesNames.Events.Constraints.PrimaryKey);
    }
}