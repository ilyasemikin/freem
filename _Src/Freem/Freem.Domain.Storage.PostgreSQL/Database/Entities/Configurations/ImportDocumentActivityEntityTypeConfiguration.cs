using Freem.Domain.Activities.Models;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class ImportDocumentActivityEntityTypeConfiguration : IEntityTypeConfiguration<ImportDocumentActivityEntity>
{
    public void Configure(EntityTypeBuilder<ImportDocumentActivityEntity> builder)
    {
        builder.ToTable(EntitiesNames.ImportDocumentActivities.Table, table =>
            table.HasCheckConstraint(
                EntitiesNames.ImportDocumentActivities.Constraints.ResolutionCheck,
                $"NOT ({EntitiesNames.ImportDocumentActivities.Properties.CreateNew} AND {EntitiesNames.ImportDocumentActivities.Properties.ResolvedActivityId} IS NOT NULL)"));

        builder
            .Property(e => e.DocumentId)
            .HasColumnName(EntitiesNames.ImportDocumentActivities.Properties.DocumentId)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.SourceId)
            .HasColumnName(EntitiesNames.ImportDocumentActivities.Properties.SourceId)
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
            .Property(e => e.CreateNew)
            .HasColumnName(EntitiesNames.ImportDocumentActivities.Properties.CreateNew)
            .HasDefaultValue(false)
            .IsRequired();

        builder
            .Property(e => e.ResolvedActivityId)
            .HasColumnName(EntitiesNames.ImportDocumentActivities.Properties.ResolvedActivityId);

        builder
            .HasKey(e => new { e.DocumentId, e.SourceId })
            .HasName(EntitiesNames.ImportDocumentActivities.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.ResolvedActivityId)
            .HasDatabaseName(EntitiesNames.ImportDocumentActivities.Constraints.ResolvedActivityIdIndex);

        builder
            .HasOne(e => e.Document)
            .WithMany(e => e.Activities)
            .HasForeignKey(e => e.DocumentId)
            .HasConstraintName(EntitiesNames.ImportDocumentActivities.Constraints.DocumentsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne(e => e.ResolvedActivity)
            .WithMany()
            .HasForeignKey(e => e.ResolvedActivityId)
            .HasConstraintName(EntitiesNames.ImportDocumentActivities.Constraints.ActivitiesForeignKey)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
