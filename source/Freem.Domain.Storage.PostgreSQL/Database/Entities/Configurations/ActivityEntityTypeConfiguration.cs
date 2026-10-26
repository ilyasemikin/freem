using Freem.Domain.Activities.Models;
using Freem.Domain.Storage.PostgreSQL.Database.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class ActivityEntityTypeConfiguration : IEntityTypeConfiguration<ActivityEntity>
{
    public void Configure(EntityTypeBuilder<ActivityEntity> builder)
    {
        builder.ToTable(EntitiesNames.Activities.Table);

        builder
            .Property(e => e.Id)
            .HasColumnName(EntitiesNames.Activities.Properties.Id)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.UserId)
            .HasColumnName(EntitiesNames.Activities.Properties.UserId)
            .IsRequired();

        builder
            .Property(e => e.Name)
            .HasMaxLength(ActivityName.MaxLength)
            .HasColumnName(EntitiesNames.Activities.Properties.Name)
            .HasDefaultValue("Default")
            .IsRequired();

        builder
            .Property(e => e.Color)
            .HasMaxLength(ActivityColor.Length)
            .HasColumnName(EntitiesNames.Activities.Properties.Color)
            .IsRequired();

        builder
            .Property(e => e.Status)
            .HasColumnName(EntitiesNames.Activities.Properties.Status)
            .HasColumnType($"{EnvironmentNames.Schema}.{EntitiesNames.Activities.Models.Status}")
            .IsRequired();

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(EntitiesNames.Activities.Properties.CreatedAt)
            .IsRequired();

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName(EntitiesNames.Activities.Properties.UpdatedAt);

        builder
            .HasKey(e => e.Id)
            .HasName(EntitiesNames.Activities.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.UserId)
            .HasDatabaseName(EntitiesNames.Activities.Constraints.UserIdIndex);

        builder
            .HasIndex(e => new { e.Name, e.UserId })
            .IsUnique()
            .HasDatabaseName(EntitiesNames.Activities.Constraints.NameUnique);

        builder
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .HasConstraintName(EntitiesNames.Activities.Constraints.UsersForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasMany(e => e.Tags)
            .WithMany(e => e.Activities)
            .UsingEntity<ActivityTagRelationEntity>();
    }
}
