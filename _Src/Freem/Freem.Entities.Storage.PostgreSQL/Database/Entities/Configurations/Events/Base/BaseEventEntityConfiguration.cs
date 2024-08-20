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
            .ToTable(EntitiesNames.Events.Table);

        builder
            .HasDiscriminator(e => e.EventType)
            .HasValue<CategoryEventEntity>(EntitiesNames.Events.Categories.EventType)
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
            .HasColumnType(EntitiesNames.Events.Models.Action)
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