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
            .Property(e => e.ImportActivityId)
            .HasColumnName(EntitiesNames.ImportDocumentActivityTags.Properties.ImportActivityId)
            .IsRequired();

        builder
            .Property(e => e.ImportTagId)
            .HasColumnName(EntitiesNames.ImportDocumentActivityTags.Properties.ImportTagId)
            .IsRequired();

        builder
            .HasKey(e => new { e.DocumentId, e.ImportActivityId, e.ImportTagId })
            .HasName(EntitiesNames.ImportDocumentActivityTags.Constraints.PrimaryKey);

        builder
            .HasOne(e => e.Activity)
            .WithMany(e => e.Tags)
            .HasForeignKey(e => new { e.DocumentId, e.ImportActivityId })
            .HasPrincipalKey(e => new { e.DocumentId, e.ImportActivityId })
            .HasConstraintName(EntitiesNames.ImportDocumentActivityTags.Constraints.ActivitiesForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
