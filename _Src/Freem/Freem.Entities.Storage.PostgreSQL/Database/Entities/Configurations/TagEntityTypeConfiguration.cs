using Freem.Entities.Storage.Abstractions.Extensions;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            .HasMaxLength(Tag.MaxNameLength)
            .IsRequired();

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(EntitiesNames.Tags.Properties.CreatedAt)
            .HasColumnOrder(1)
            .IsRequired();

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName(EntitiesNames.Tags.Properties.UpdatedAt);

        builder
            .HasKey(e => e.Id)
            .HasName(EntitiesNames.Tags.Constaints.PrimaryKey);

        builder
            .HasIndex(e => e.UserId)
            .HasDatabaseName(EntitiesNames.Tags.Constaints.UserIdIndex);

        builder
            .HasIndex(e => new { e.Name, e.UserId })
            .HasDatabaseName(EntitiesNames.Tags.Constaints.NameUserIdIndex)
            .IsUnique()
            .HasDatabaseName(EntitiesNames.Tags.Constaints.NameUserIdUnique);

        builder
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .HasConstraintName(EntitiesNames.Tags.Constaints.UsersForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasMany(e => e.Categories)
            .WithMany(e => e.Tags)
            .UsingEntity<CategoryTagRelationEntity>();

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
