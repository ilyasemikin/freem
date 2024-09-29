using Freem.Entities.Activities.Events;
using Freem.Entities.Records.Events;
using Freem.Entities.RunningRecords.Events;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Tags.Events;
using Freem.Entities.Users.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class EventEntityTypeConfiguration : IEntityTypeConfiguration<EventEntity>
{
    public void Configure(EntityTypeBuilder<EventEntity> builder)
    {
        builder.ToTable(EntitiesNames.Events.Table, table =>
            table.HasCheckConstraint(
                EntitiesNames.Events.Constraints.EntityNameCheck,
                $"{EntitiesNames.Events.Properties.EntityName} = '{EntitiesNames.Activities.EntityName}' and ({BuildActionCheckRawSql(ActivityEventActions.All)}) or " +
                $"{EntitiesNames.Events.Properties.EntityName} = '{EntitiesNames.Records.EntityName}' and ({BuildActionCheckRawSql(RecordEventActions.All)}) or " +
                $"{EntitiesNames.Events.Properties.EntityName} = '{EntitiesNames.RunningRecords.EntityName}' and ({BuildActionCheckRawSql(RunningRecordEventActions.All)}) or " +
                $"{EntitiesNames.Events.Properties.EntityName} = '{EntitiesNames.Tags.EntityName}' and ({BuildActionCheckRawSql(TagEventActions.All)}) or " +
                $"{EntitiesNames.Events.Properties.EntityName} = '{EntitiesNames.Users.EntityName}' and ({BuildActionCheckRawSql(UserEventActions.All)})"));

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
            .Property(e => e.EntityName)
            .HasColumnName(EntitiesNames.Events.Properties.EntityName)
            .IsRequired();

        builder
            .Property(e => e.EntityId)
            .HasColumnName(EntitiesNames.Events.Properties.EntityId)
            .IsRequired();

        builder
            .Property(e => e.Action)
            .HasColumnName(EntitiesNames.Events.Properties.Action)
            .IsRequired();

        builder
            .Property(e => e.AdditionalData)
            .HasColumnName(EntitiesNames.Events.Properties.AdditionalData)
            .HasColumnType("jsonb");

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(EntitiesNames.Events.Properties.CreatedAt)
            .HasColumnOrder(2)
            .IsRequired();

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName(EntitiesNames.Events.Properties.UpdatedAt);
        
        builder
            .HasKey(e => e.Id)
            .HasName(EntitiesNames.Events.Constraints.PrimaryKey);
    }

    private static string BuildActionCheckRawSql(IEnumerable<string> values)
    {
        values = values.Select(value => $"{EntitiesNames.Events.Properties.Action} = '{value}'");
        return string.Join(" or ", values);
    }
}