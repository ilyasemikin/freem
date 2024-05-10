using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;

internal class RecordEntityTypeConfiguration : IEntityTypeConfiguration<RecordEntity>
{
    public void Configure(EntityTypeBuilder<RecordEntity> builder)
    {
        builder.ToTable(
            Names.Tables.Records, 
            Names.Schema,
            table => table
                .HasCheckConstraint(
                    Names.Constrains.Records.TimeRangeCheck, 
                    $"{Names.Properties.Records.StartAt} <= {Names.Properties.Records.EndAt}"));

        builder
            .Property(e => e.Id)
            .HasColumnName(Names.Properties.Records.Id)
            .IsRequired();

        builder
            .Property(e => e.StartAt)
            .HasColumnType(Names.Properties.Records.StartAt)
            .IsRequired();

        builder
            .Property(e => e.EndAt)
            .HasColumnType(Names.Properties.Records.EndAt)
            .IsRequired();

        builder
            .Property(e => e.Name)
            .HasColumnName(Names.Properties.Records.Name);

        builder
            .Property(e => e.Description)
            .HasColumnName(Names.Properties.Records.Description);

        builder
            .HasKey(e => e.Id)
            .HasName(Names.Constrains.Records.PrimaryKey);

        builder
            .HasMany(e => e.Categories)
            .WithMany()
            .UsingEntity<RecordCategoryRelation>();

        builder
            .HasMany(e => e.Tags)
            .WithMany()
            .UsingEntity<RecordTagRelation>();
    }
}
