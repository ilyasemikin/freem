using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Relations.Configurations;

internal class RecordCategoryRelationEntityTypeConfiguration : IEntityTypeConfiguration<RecordCategoryRelation>
{
    public void Configure(EntityTypeBuilder<RecordCategoryRelation> builder)
    {
        builder.ToTable(Names.Relations.Tables.RecordsCategories, Names.Schema);

        builder
            .Property(e => e.RecordId)
            .HasColumnName(Names.Relations.Properties.RecordsCategories.RecordId)
            .IsRequired();

        builder
            .Property(e => e.CategoryId)
            .HasColumnName(Names.Relations.Properties.RecordsCategories.CategoryId)
            .IsRequired();

        builder
            .HasKey(e => new { e.RecordId, e.CategoryId })
            .HasName(Names.Relations.Constrains.RecordsCategories.PrimaryKey);

        builder
            .HasIndex(e => e.RecordId)
            .HasDatabaseName(Names.Relations.Constrains.RecordsCategories.RecordIdIndex);

        builder
            .HasIndex(e => e.CategoryId)
            .HasDatabaseName(Names.Relations.Constrains.RecordsCategories.CategoryIdIndex);

        builder
            .HasOne(e => e.Record)
            .WithMany()
            .HasForeignKey(e => e.RecordId)
            .HasConstraintName(Names.Relations.Constrains.RecordsCategories.RecordsForeignKey)
            .IsRequired();

        builder
            .HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .HasConstraintName(Names.Relations.Constrains.RecordsCategories.CategoriesForeignKey)
            .IsRequired();
    }
}