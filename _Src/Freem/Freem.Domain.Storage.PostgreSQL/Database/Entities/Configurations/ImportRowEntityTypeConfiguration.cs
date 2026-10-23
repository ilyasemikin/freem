using Freem.Domain.Records.Models;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class ImportRowEntityTypeConfiguration : IEntityTypeConfiguration<ImportRowEntity>
{
    public void Configure(EntityTypeBuilder<ImportRowEntity> builder)
    {
        builder.ToTable(EntitiesNames.ImportRows.Table);

        builder
            .Property(e => e.Id)
            .HasColumnName(EntitiesNames.ImportRows.Properties.Id)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.DocumentId)
            .HasColumnName(EntitiesNames.ImportRows.Properties.DocumentId)
            .IsRequired();

        builder
            .Property(e => e.SourceId)
            .HasColumnName(EntitiesNames.ImportRows.Properties.SourceId)
            .IsRequired();

        builder
            .Property(e => e.Name)
            .HasColumnName(EntitiesNames.ImportRows.Properties.Name)
            .HasMaxLength(RecordName.MaxLength);

        builder
            .Property(e => e.Description)
            .HasColumnName(EntitiesNames.ImportRows.Properties.Description)
            .HasMaxLength(RecordDescription.MaxLength);

        builder
            .Property(e => e.FirstStartAt)
            .HasColumnName(EntitiesNames.ImportRows.Properties.FirstStartAt)
            .IsRequired();

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(EntitiesNames.ImportRows.Properties.CreatedAt).IsRequired();

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName(EntitiesNames.ImportRows.Properties.UpdatedAt);

        builder
            .HasKey(e => e.Id)
            .HasName(EntitiesNames.ImportRows.Constraints.PrimaryKey);

        builder
            .HasIndex(e => new { e.DocumentId, e.FirstStartAt, e.Id })
            .HasDatabaseName(EntitiesNames.ImportRows.Constraints.DocumentOrderIndex);

        builder
            .HasOne(e => e.Document)
            .WithMany()
            .HasForeignKey(e => e.DocumentId)
            .HasConstraintName(EntitiesNames.ImportRows.Constraints.DocumentsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
