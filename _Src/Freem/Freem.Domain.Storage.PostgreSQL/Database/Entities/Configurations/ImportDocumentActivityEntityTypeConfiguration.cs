using Freem.Domain.Activities.Models;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class ImportDocumentActivityEntityTypeConfiguration : IEntityTypeConfiguration<ImportDocumentActivityEntity>
{
    public void Configure(EntityTypeBuilder<ImportDocumentActivityEntity> builder)
    {
        builder.ToTable(EntitiesNames.ImportDocumentActivities.Table);

        builder
            .Property(e => e.DocumentId)
            .HasColumnName(EntitiesNames.ImportDocumentActivities.Properties.DocumentId)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.ImportActivityId)
            .HasColumnName(EntitiesNames.ImportDocumentActivities.Properties.ImportActivityId)
            .IsRequired();

        builder
            .Property(e => e.Name)
            .HasColumnName(EntitiesNames.ImportDocumentActivities.Properties.Name)
            .HasMaxLength(ActivityName.MaxLength)
            .IsRequired();

        builder
            .Property(e => e.Color)
            .HasColumnName(EntitiesNames.ImportDocumentActivities.Properties.Color)
            .HasMaxLength(ActivityColor.Length);

        builder
            .Property(e => e.Status)
            .HasColumnName(EntitiesNames.ImportDocumentActivities.Properties.Status)
            .HasMaxLength(32)
            .IsRequired();

        builder
            .Property(e => e.MappedActivityId)
            .HasColumnName(EntitiesNames.ImportDocumentActivities.Properties.MappedActivityId);

        builder
            .HasKey(e => new { e.DocumentId, e.ImportActivityId })
            .HasName(EntitiesNames.ImportDocumentActivities.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.MappedActivityId)
            .HasDatabaseName(EntitiesNames.ImportDocumentActivities.Constraints.MappedActivityIdIndex);

        builder
            .HasOne(e => e.Document)
            .WithMany(e => e.Activities)
            .HasForeignKey(e => e.DocumentId)
            .HasConstraintName(EntitiesNames.ImportDocumentActivities.Constraints.DocumentsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne(e => e.MappedActivity)
            .WithMany()
            .HasForeignKey(e => e.MappedActivityId)
            .HasConstraintName(EntitiesNames.ImportDocumentActivities.Constraints.ActivitiesForeignKey)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
