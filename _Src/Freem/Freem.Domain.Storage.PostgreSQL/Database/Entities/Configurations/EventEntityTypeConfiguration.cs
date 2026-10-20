using Freem.Domain;
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
                EntitiesNames.Events.Constraints.EntityTypeCheck,
                BuildEntityTypeCheckRawSql()));

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
            .Property(e => e.OccurredAt)
            .HasColumnName(EntitiesNames.Events.Properties.OccurredAt)
            .HasColumnOrder(2)
            .IsRequired();

        builder
            .Property(e => e.EventType)
            .HasColumnName(EntitiesNames.Events.Properties.EventType)
            .IsRequired();

        builder
            .Property(e => e.EntityType)
            .HasColumnName(EntitiesNames.Events.Properties.EntityType)
            .IsRequired();

        builder
            .Property(e => e.EntityRef)
            .HasColumnName(EntitiesNames.Events.Properties.EntityRef)
            .IsRequired();

        builder
            .Property(e => e.Payload)
            .HasColumnName(EntitiesNames.Events.Properties.Payload)
            .HasColumnType("jsonb")
            .IsRequired();

        builder
            .HasKey(e => e.Id)
            .HasName(EntitiesNames.Events.Constraints.PrimaryKey);

        builder
            .HasIndex(e => new { e.UserId, e.OccurredAt })
            .HasDatabaseName(EntitiesNames.Events.Constraints.UserIdOccurredAtIndex);

        builder
            .HasIndex(e => new { e.UserId, e.EntityType, e.EntityRef })
            .HasDatabaseName(EntitiesNames.Events.Constraints.UserIdEntityIndex);
    }

    private static string BuildEntityTypeCheckRawSql()
    {
        var names = new[]
        {
            EntityNames.Activity,
            EntityNames.Record,
            EntityNames.RunningRecord,
            EntityNames.Tag,
            EntityNames.User
        };

        var conditions = names.Select(name => $"{EntitiesNames.Events.Properties.EntityType} = '{name}'");
        return string.Join(" or ", conditions);
    }
}
