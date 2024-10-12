using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Tags.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class TagEntityTypeConfiguration : IEntityTypeConfiguration<TagEntity>
{
    public void Configure(EntityTypeBuilder<TagEntity> builder)
    {
        builder.ToTable(EntitiesNames.Tags.Table);

        builder
            .Property(e => e.Id)
            .HasColumnName(EntitiesNames.Tags.Properties.Id)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.UserId)
            .HasColumnName(EntitiesNames.Tags.Properties.UserId)
            .IsRequired();

        builder
            .Property(e => e.Name)
            .HasColumnName(EntitiesNames.Tags.Properties.Name)
            .HasMaxLength(TagName.MaxLength)
            .IsRequired();

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(EntitiesNames.Tags.Properties.CreatedAt)
            .IsRequired();

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName(EntitiesNames.Tags.Properties.UpdatedAt);

        builder
            .HasKey(e => e.Id)
            .HasName(EntitiesNames.Tags.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.UserId)
            .HasDatabaseName(EntitiesNames.Tags.Constraints.UserIdIndex);

        builder
            .HasIndex(e => new { e.Name, e.UserId })
            .HasDatabaseName(EntitiesNames.Tags.Constraints.NameUserIdIndex)
            .IsUnique()
            .HasDatabaseName(EntitiesNames.Tags.Constraints.NameUserIdUnique);

        builder
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .HasConstraintName(EntitiesNames.Tags.Constraints.UsersForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasMany(e => e.Activities)
            .WithMany(e => e.Tags)
            .UsingEntity<ActivityTagRelationEntity>();

        builder
            .HasMany(e => e.Records)
            .WithMany(e => e.Tags)
            .UsingEntity<RecordTagRelationEntity>();

        builder
            .HasMany(e => e.RunningRecords)
            .WithMany(e => e.Tags)
            .UsingEntity<RunningRecordTagRelationEntity>();
    }
}
