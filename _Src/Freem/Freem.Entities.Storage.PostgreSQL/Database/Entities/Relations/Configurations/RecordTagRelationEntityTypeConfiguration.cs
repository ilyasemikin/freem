using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Configurations;

internal sealed class RecordTagRelationEntityTypeConfiguration : IEntityTypeConfiguration<RecordTagRelationEntity>
{
    public void Configure(EntityTypeBuilder<RecordTagRelationEntity> builder)
    {
        builder.ToTable(
            RelationsNames.RecordsTags.Table,
            table => table
                .HasTrigger(RelationsNames.RecordsTags.Constraints.UserIdCheckTrigger));

        builder
            .Property(e => e.RecordId)
            .HasColumnName(RelationsNames.RecordsTags.Properties.RecordId)
            .IsRequired();

        builder
            .Property(e => e.TagId)
            .HasColumnName(RelationsNames.RecordsTags.Properties.TagId)
            .IsRequired();

        builder
            .HasKey(e => new { e.RecordId, e.TagId })
            .HasName(RelationsNames.RecordsTags.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.TagId)
            .HasDatabaseName(RelationsNames.RecordsTags.Constraints.TagIdIndex);

        builder
            .HasOne(e => e.Record)
            .WithMany()
            .HasForeignKey(e => e.RecordId)
            .HasConstraintName(RelationsNames.RecordsTags.Constraints.RecordsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne(e => e.Tag)
            .WithMany()
            .HasForeignKey(e => e.TagId)
            .HasConstraintName(RelationsNames.RecordsTags.Constraints.TagsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
