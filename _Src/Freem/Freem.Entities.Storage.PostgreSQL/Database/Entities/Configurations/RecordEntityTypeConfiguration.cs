using Freem.Entities.Records.Models;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class RecordEntityTypeConfiguration : IEntityTypeConfiguration<RecordEntity>
{
    public void Configure(EntityTypeBuilder<RecordEntity> builder)
    {
        builder.ToTable(EntitiesNames.Records.Table, table =>
            table.HasCheckConstraint(
                EntitiesNames.Records.Constraints.TimePeriodCheck, 
                $"{EntitiesNames.Records.Properties.StartAt} <= {EntitiesNames.Records.Properties.EndAt}"));

        builder
            .Property(e => e.Id)
            .HasColumnName(EntitiesNames.Records.Properties.Id)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.UserId)
            .HasColumnName(EntitiesNames.Records.Properties.UserId)
            .IsRequired();

        builder
            .Property(e => e.Name)
            .HasMaxLength(RecordName.MaxLength)
            .HasColumnName(EntitiesNames.Records.Properties.Name);

        builder
            .Property(e => e.Description)
            .HasMaxLength(RecordDescription.MaxLength)
            .HasColumnName(EntitiesNames.Records.Properties.Description);

        builder
            .Property(e => e.StartAt)
            .HasColumnName(EntitiesNames.Records.Properties.StartAt)
            .IsRequired();

        builder
            .Property(e => e.EndAt)
            .HasColumnName(EntitiesNames.Records.Properties.EndAt)
            .IsRequired();

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(EntitiesNames.Records.Properties.CreatedAt)
            .IsRequired();

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName(EntitiesNames.Records.Properties.UpdatedAt);

        builder
            .HasKey(e => e.Id)
            .HasName(EntitiesNames.Records.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.UserId)
            .HasDatabaseName(EntitiesNames.Records.Constraints.UserIdIndex);

        builder
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .HasConstraintName(EntitiesNames.Records.Constraints.UsersForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasMany(e => e.Activities)
            .WithMany()
            .UsingEntity<RecordActivityRelationEntity>();

        builder
            .HasMany(e => e.Tags)
            .WithMany(e => e.Records)
            .UsingEntity<RecordTagRelationEntity>();
    }
}
