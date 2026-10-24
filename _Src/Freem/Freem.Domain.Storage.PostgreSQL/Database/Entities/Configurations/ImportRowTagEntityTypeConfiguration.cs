using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class ImportRowTagEntityTypeConfiguration : IEntityTypeConfiguration<ImportRowTagEntity>
{
    public void Configure(EntityTypeBuilder<ImportRowTagEntity> builder)
    {
        builder.ToTable(EntitiesNames.ImportRowTags.Table);

        builder
            .Property(e => e.RowId)
            .HasColumnName(EntitiesNames.ImportRowTags.Properties.RowId)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.ImportTagId)
            .HasColumnName(EntitiesNames.ImportRowTags.Properties.ImportTagId)
            .IsRequired();

        builder
            .Property(e => e.Position)
            .HasColumnName(EntitiesNames.ImportRowTags.Properties.Position)
            .IsRequired();

        builder
            .HasKey(e => new { e.RowId, e.ImportTagId })
            .HasName(EntitiesNames.ImportRowTags.Constraints.PrimaryKey);

        builder
            .HasOne(e => e.Row)
            .WithMany(e => e.Tags)
            .HasForeignKey(e => e.RowId)
            .HasConstraintName(EntitiesNames.ImportRowTags.Constraints.RowsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
