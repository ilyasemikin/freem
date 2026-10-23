using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Domain.Tags.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class ImportDocumentTagEntityTypeConfiguration : IEntityTypeConfiguration<ImportDocumentTagEntity>
{
    public void Configure(EntityTypeBuilder<ImportDocumentTagEntity> builder)
    {
        builder.ToTable(EntitiesNames.ImportDocumentTags.Table, table =>
            table.HasCheckConstraint(
                EntitiesNames.ImportDocumentTags.Constraints.ResolutionCheck,
                $"NOT ({EntitiesNames.ImportDocumentTags.Properties.CreateNew} AND {EntitiesNames.ImportDocumentTags.Properties.ResolvedTagId} IS NOT NULL)"));

        builder
            .Property(e => e.DocumentId)
            .HasColumnName(EntitiesNames.ImportDocumentTags.Properties.DocumentId)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.SourceId)
            .HasColumnName(EntitiesNames.ImportDocumentTags.Properties.SourceId)
            .IsRequired();

        builder
            .Property(e => e.Name)
            .HasColumnName(EntitiesNames.ImportDocumentTags.Properties.Name)
            .HasMaxLength(TagName.MaxLength)
            .IsRequired();

        builder
            .Property(e => e.CreateNew)
            .HasColumnName(EntitiesNames.ImportDocumentTags.Properties.CreateNew)
            .HasDefaultValue(false)
            .IsRequired();

        builder
            .Property(e => e.ResolvedTagId)
            .HasColumnName(EntitiesNames.ImportDocumentTags.Properties.ResolvedTagId);

        builder
            .HasKey(e => new { e.DocumentId, e.SourceId })
            .HasName(EntitiesNames.ImportDocumentTags.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.ResolvedTagId)
            .HasDatabaseName(EntitiesNames.ImportDocumentTags.Constraints.ResolvedTagIdIndex);

        builder
            .HasOne(e => e.Document)
            .WithMany(e => e.Tags)
            .HasForeignKey(e => e.DocumentId)
            .HasConstraintName(EntitiesNames.ImportDocumentTags.Constraints.DocumentsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne(e => e.ResolvedTag)
            .WithMany()
            .HasForeignKey(e => e.ResolvedTagId)
            .HasConstraintName(EntitiesNames.ImportDocumentTags.Constraints.TagsForeignKey)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
