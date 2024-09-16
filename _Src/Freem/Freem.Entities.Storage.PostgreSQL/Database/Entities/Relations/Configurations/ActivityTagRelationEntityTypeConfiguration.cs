using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Configurations;

internal sealed class ActivityTagRelationEntityTypeConfiguration : IEntityTypeConfiguration<ActivityTagRelationEntity>
{
    public void Configure(EntityTypeBuilder<ActivityTagRelationEntity> builder)
    {
        builder.ToTable(RelationsNames.ActivitiesTags.Table);

        builder
            .Property(e => e.ActivityId)
            .HasColumnName(RelationsNames.ActivitiesTags.Properties.ActivityId)
            .IsRequired();

        builder
            .Property(e => e.TagId)
            .HasColumnName(RelationsNames.ActivitiesTags.Properties.TagId)
            .IsRequired();

        builder
            .HasKey(e => new { e.ActivityId, e.TagId })
            .HasName(RelationsNames.ActivitiesTags.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.TagId)
            .HasDatabaseName(RelationsNames.ActivitiesTags.Constraints.TagIdIndex);

        builder
            .HasOne(e => e.Activity)
            .WithMany()
            .HasConstraintName(RelationsNames.ActivitiesTags.Constraints.ActivitiesForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne(e => e.Tag)
            .WithMany()
            .HasConstraintName(RelationsNames.ActivitiesTags.Constraints.TagsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
