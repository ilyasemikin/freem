using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class ImportRowActivityEntityTypeConfiguration : IEntityTypeConfiguration<ImportRowActivityEntity>
{
    public void Configure(EntityTypeBuilder<ImportRowActivityEntity> builder)
    {
        builder.ToTable(EntitiesNames.ImportRowActivities.Table);

        builder
            .Property(e => e.RowId)
            .HasColumnName(EntitiesNames.ImportRowActivities.Properties.RowId)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.SourceActivityId)
            .HasColumnName(EntitiesNames.ImportRowActivities.Properties.SourceActivityId)
            .IsRequired();

        builder
            .Property(e => e.Position)
            .HasColumnName(EntitiesNames.ImportRowActivities.Properties.Position)
            .IsRequired();

        builder
            .HasKey(e => new { e.RowId, e.SourceActivityId })
            .HasName(EntitiesNames.ImportRowActivities.Constraints.PrimaryKey);

        builder
            .HasOne(e => e.Row)
            .WithMany(e => e.Activities)
            .HasForeignKey(e => e.RowId)
            .HasConstraintName(EntitiesNames.ImportRowActivities.Constraints.RowsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
