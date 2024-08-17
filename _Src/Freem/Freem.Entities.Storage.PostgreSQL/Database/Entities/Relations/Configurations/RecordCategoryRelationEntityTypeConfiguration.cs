using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Configurations;

internal sealed class RecordCategoryRelationEntityTypeConfiguration : IEntityTypeConfiguration<RecordCategoryRelationEntity>
{
    public void Configure(EntityTypeBuilder<RecordCategoryRelationEntity> builder)
    {
        builder.ToTable(RelationsNames.RecordsCategories.Table);

        builder
            .Property(e => e.RecordId)
            .HasColumnName(RelationsNames.RecordsCategories.Properties.RecordId)
            .IsRequired();

        builder
            .Property(e => e.CategoryId)
            .HasColumnName(RelationsNames.RecordsCategories.Properties.CategoryId)
            .IsRequired();

        builder
            .HasKey(e => new { e.RecordId, e.CategoryId })
            .HasName(RelationsNames.RecordsCategories.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.CategoryId)
            .HasDatabaseName(RelationsNames.RecordsCategories.Constraints.CategoryIdIndex);

        builder
            .HasOne(e => e.Record)
            .WithMany()
            .HasForeignKey(e => e.RecordId)
            .HasConstraintName(RelationsNames.RecordsCategories.Constraints.RecordsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .HasConstraintName(RelationsNames.RecordsCategories.Constraints.CategoriesForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
