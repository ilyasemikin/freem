using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Relations.Configurations;

internal class CategoryTagRelationEntityTypeConfiguration : IEntityTypeConfiguration<CategoryTagRelation>
{
    public void Configure(EntityTypeBuilder<CategoryTagRelation> builder)
    {
        builder.ToTable(Names.Relations.Tables.CategoriesTags, Names.Schema);

        builder
            .Property(e => e.CategoryId)
            .HasColumnName(Names.Relations.Properties.CategoriesTags.CategoryId)
            .IsRequired();

        builder
            .Property(e => e.TagId)
            .HasColumnName(Names.Relations.Properties.CategoriesTags.TagId)
            .IsRequired();

        builder
            .HasKey(e => new { e.CategoryId, e.TagId })
            .HasName(Names.Relations.Constrains.CategoriesTags.PrimaryKey);

        builder
            .HasIndex(e => e.CategoryId)
            .HasDatabaseName(Names.Relations.Constrains.CategoriesTags.CategoryIdIndex);

        builder
            .HasIndex(e => e.TagId)
            .HasDatabaseName(Names.Relations.Constrains.CategoriesTags.TagsForeignKey);

        builder
            .HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .HasConstraintName(Names.Relations.Constrains.CategoriesTags.CategoriesForeignKey)
            .IsRequired();

        builder
            .HasOne(e => e.Tag)
            .WithMany()
            .HasForeignKey(e => e.TagId)
            .HasConstraintName(Names.Relations.Constrains.CategoriesTags.TagsForeignKey)
            .IsRequired();
    }
}