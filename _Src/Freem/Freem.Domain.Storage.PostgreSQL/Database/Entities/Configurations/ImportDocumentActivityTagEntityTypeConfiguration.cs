using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class ImportDocumentActivityTagEntityTypeConfiguration : IEntityTypeConfiguration<ImportDocumentActivityTagEntity>
{
    public void Configure(EntityTypeBuilder<ImportDocumentActivityTagEntity> builder)
    {
        builder.ToTable(EntitiesNames.ImportDocumentActivityTags.Table);

        builder
            .Property(e => e.DocumentId)
            .HasColumnName(EntitiesNames.ImportDocumentActivityTags.Properties.DocumentId)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.ActivitySourceId)
            .HasColumnName(EntitiesNames.ImportDocumentActivityTags.Properties.ActivitySourceId)
            .IsRequired();

        builder
            .Property(e => e.TagSourceId)
            .HasColumnName(EntitiesNames.ImportDocumentActivityTags.Properties.TagSourceId)
            .IsRequired();

        builder
            .HasKey(e => new { e.DocumentId, e.ActivitySourceId, e.TagSourceId })
            .HasName(EntitiesNames.ImportDocumentActivityTags.Constraints.PrimaryKey);

        builder
            .HasOne(e => e.Activity)
            .WithMany(e => e.Tags)
            .HasForeignKey(e => new { e.DocumentId, e.ActivitySourceId })
            .HasPrincipalKey(e => new { e.DocumentId, e.SourceId })
            .HasConstraintName(EntitiesNames.ImportDocumentActivityTags.Constraints.ActivitiesForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
