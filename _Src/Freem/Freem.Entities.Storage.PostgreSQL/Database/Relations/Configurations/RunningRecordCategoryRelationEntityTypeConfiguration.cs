using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Relations.Configurations;

internal class RunningRecordCategoryRelationEntityTypeConfiguration : IEntityTypeConfiguration<RunningRecordCategoryRelation>
{
    public void Configure(EntityTypeBuilder<RunningRecordCategoryRelation> builder)
    {
        builder.ToTable(Names.Relations.Tables.RunningRecordCategories, Names.Schema);

        builder
            .Property(e => e.RunningRecordUserId)
            .HasColumnName(Names.Relations.Properties.RunningRecordsCategories.RunningRecordUserId)
            .IsRequired();

        builder
            .Property(e => e.CategoryId)
            .HasColumnName(Names.Relations.Properties.RunningRecordsCategories.CategoryId)
            .IsRequired();

        builder
            .HasKey(e => new { e.RunningRecordUserId, e.CategoryId })
            .HasName(Names.Relations.Constrains.RunningRecordsCategories.PrimaryKey);

        builder
            .HasIndex(e => e.RunningRecordUserId)
            .HasDatabaseName(Names.Relations.Constrains.RunningRecordsCategories.RunningRecordUserIdIndex);

        builder
            .HasIndex(e => e.CategoryId)
            .HasDatabaseName(Names.Relations.Constrains.RunningRecordsCategories.CategoryIdIndex);

        builder
            .HasOne(e => e.RunningRecord)
            .WithMany()
            .HasForeignKey(e => e.RunningRecordUserId)
            .HasConstraintName(Names.Relations.Constrains.RunningRecordsCategories.RunningRecordsForeignKey)
            .IsRequired();

        builder
            .HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .HasConstraintName(Names.Relations.Constrains.RunningRecordsCategories.CategoriesForeignKey)
            .IsRequired();
    }
}