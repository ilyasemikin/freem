using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;

internal class RunningRecordEntityTypeConfiguration : IEntityTypeConfiguration<RunningRecordEntity>
{
    public void Configure(EntityTypeBuilder<RunningRecordEntity> builder)
    {
        builder.ToTable(Names.Tables.RunningRecords, Names.Schema);

        builder
            .Property(e => e.UserId)
            .HasColumnName(Names.Properties.RunningRecords.UserId)
            .IsRequired();

        builder
            .Property(e => e.Name)
            .HasColumnName(Names.Properties.RunningRecords.Name);

        builder
            .Property(e => e.Description)
            .HasColumnName(Names.Properties.RunningRecords.Description);

        builder
            .Property(e => e.StartAt)
            .HasColumnName(Names.Properties.RunningRecords.StartAt)
            .IsRequired();

        builder
            .HasKey(e => e.UserId)
            .HasName(Names.Constrains.RunningRecords.PrimaryKey);

        builder
            .HasOne(e => e.User)
            .WithOne()
            .HasForeignKey<RunningRecordEntity>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(Names.Constrains.RunningRecords.UsersForeignKey)
            .IsRequired();

        builder
            .HasMany(e => e.Categories)
            .WithMany()
            .UsingEntity<RunningRecordCategoryRelation>();

        builder
            .HasMany(e => e.TagIds)
            .WithMany()
            .UsingEntity<RunningRecordTagRelation>();
    }
}
