using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Domain.Tags.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class ImportDocumentTagEntityTypeConfiguration : IEntityTypeConfiguration<ImportDocumentTagEntity>
{
    public void Configure(EntityTypeBuilder<ImportDocumentTagEntity> builder)
    {
        builder.ToTable(EntitiesNames.ImportDocumentTags.Table);

        builder
            .Property(e => e.DocumentId)
            .HasColumnName(EntitiesNames.ImportDocumentTags.Properties.DocumentId)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.ImportTagId)
            .HasColumnName(EntitiesNames.ImportDocumentTags.Properties.ImportTagId)
            .IsRequired();

        builder
            .Property(e => e.Name)
            .HasColumnName(EntitiesNames.ImportDocumentTags.Properties.Name)
            .HasMaxLength(TagName.MaxLength)
            .IsRequired();

        builder
            .Property(e => e.MappedTagId)
            .HasColumnName(EntitiesNames.ImportDocumentTags.Properties.MappedTagId);

        builder
            .HasKey(e => new { e.DocumentId, e.ImportTagId })
            .HasName(EntitiesNames.ImportDocumentTags.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.MappedTagId)
            .HasDatabaseName(EntitiesNames.ImportDocumentTags.Constraints.MappedTagIdIndex);

        builder
            .HasOne(e => e.Document)
            .WithMany(e => e.Tags)
            .HasForeignKey(e => e.DocumentId)
            .HasConstraintName(EntitiesNames.ImportDocumentTags.Constraints.DocumentsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne(e => e.MappedTag)
            .WithMany()
            .HasForeignKey(e => e.MappedTagId)
            .HasConstraintName(EntitiesNames.ImportDocumentTags.Constraints.TagsForeignKey)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
