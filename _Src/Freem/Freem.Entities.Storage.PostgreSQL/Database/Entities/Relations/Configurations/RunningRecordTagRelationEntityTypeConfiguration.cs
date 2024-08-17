using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Configurations;

internal sealed class RunningRecordTagRelationEntityTypeConfiguration : IEntityTypeConfiguration<RunningRecordTagRelationEntity>
{
    public void Configure(EntityTypeBuilder<RunningRecordTagRelationEntity> builder)
    {
        builder.ToTable(RelationsNames.RunningRecordsTags.Table);

        builder
            .Property(e => e.RunningRecordUserId)
            .HasColumnName(RelationsNames.RunningRecordsTags.Properties.RunningRecordId)
            .IsRequired();

        builder
            .Property(e => e.TagId)
            .HasColumnName(RelationsNames.RunningRecordsTags.Properties.TagId)
            .IsRequired();

        builder
            .HasKey(e => new { e.RunningRecordUserId, e.TagId })
            .HasName(RelationsNames.RunningRecordsTags.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.TagId)
            .HasDatabaseName(RelationsNames.RunningRecordsTags.Constraints.TagIdIndex);

        builder
            .HasOne(e => e.RunningRecord)
            .WithMany()
            .HasForeignKey(e => e.RunningRecordUserId)
            .HasConstraintName(RelationsNames.RunningRecordsTags.Constraints.RunningRecordsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne(e => e.Tag)
            .WithMany()
            .HasForeignKey(e => e.TagId)
            .HasConstraintName(RelationsNames.RunningRecordsTags.Constraints.TagsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
