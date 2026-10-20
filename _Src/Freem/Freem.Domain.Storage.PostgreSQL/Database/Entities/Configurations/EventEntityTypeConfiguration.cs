using Freem.Domain;
using Freem.Domain.Activities.Events;
using Freem.Domain.Records.Events;
using Freem.Domain.RunningRecords.Events;
using Freem.Domain.Tags.Events;
using Freem.Domain.Users.Events;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class EventEntityTypeConfiguration : IEntityTypeConfiguration<EventEntity>
{
    public void Configure(EntityTypeBuilder<EventEntity> builder)
    {
        builder.ToTable(EntitiesNames.Events.Table, table =>
            table.HasCheckConstraint(
                EntitiesNames.Events.Constraints.EntityNameCheck,
                $"{EntitiesNames.Events.Properties.EntityName} = '{EntityNames.Activity}' and ({BuildActionCheckRawSql(ActivityEventActions.All)}) or " +
                $"{EntitiesNames.Events.Properties.EntityName} = '{EntityNames.Record}' and ({BuildActionCheckRawSql(RecordEventActions.All)}) or " +
                $"{EntitiesNames.Events.Properties.EntityName} = '{EntityNames.RunningRecord}' and ({BuildActionCheckRawSql(RunningRecordEventActions.All)}) or " +
                $"{EntitiesNames.Events.Properties.EntityName} = '{EntityNames.Tag}' and ({BuildActionCheckRawSql(TagEventActions.All)}) or " +
                $"{EntitiesNames.Events.Properties.EntityName} = '{EntityNames.User}' and ({BuildActionCheckRawSql(UserEventActions.All)})"));

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