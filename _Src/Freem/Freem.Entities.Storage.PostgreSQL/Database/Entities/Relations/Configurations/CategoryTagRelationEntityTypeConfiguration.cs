using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Configurations;

internal sealed class CategoryTagRelationEntityTypeConfiguration : IEntityTypeConfiguration<CategoryTagRelationEntity>
{
    public void Configure(EntityTypeBuilder<CategoryTagRelationEntity> builder)
    {
        builder.ToTable(
            RelationsNames.CategoriesTags.Table,
            table => table
                .HasTrigger(RelationsNames.CategoriesTags.Constraints.UserIdCheckTrigger));

        builder
            .Property(e => e.CategoryId)
            .HasColumnName(RelationsNames.CategoriesTags.Properties.CategoryId)
            .IsRequired();

        builder
            .Property(e => e.TagId)
            .HasColumnName(RelationsNames.CategoriesTags.Properties.TagId)
            .IsRequired();

        builder
            .HasKey(e => new { e.CategoryId, e.TagId })
            .HasName(RelationsNames.CategoriesTags.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.TagId)
            .HasDatabaseName(RelationsNames.CategoriesTags.Constraints.TagIdIndex);

        builder
            .HasOne(e => e.Category)
            .WithMany()
            .HasConstraintName(RelationsNames.CategoriesTags.Constraints.CategoriesForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne(e => e.Tag)
            .WithMany()
            .HasConstraintName(RelationsNames.CategoriesTags.Constraints.TagsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
