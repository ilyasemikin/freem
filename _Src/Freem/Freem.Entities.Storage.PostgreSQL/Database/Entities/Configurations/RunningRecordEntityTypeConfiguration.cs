using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;

internal class RunningRecordEntityTypeConfiguration : IEntityTypeConfiguration<RunningRecordEntity>
{
    public void Configure(EntityTypeBuilder<RunningRecordEntity> builder)
    {
        builder
            .Property(e => e.UserId)
            .IsRequired();

        builder
            .HasKey(e => e.UserId);

        builder
            .Property(e => e.Name);

        builder
            .Property(e => e.Description);

        builder
            .Property(e => e.StartAt)
            .IsRequired();

        builder
            .HasOne(e => e.User)
            .WithOne()
            .HasForeignKey<RunningRecordEntity>(e => e.UserId)
            .IsRequired();

        builder
            .HasMany(e => e.Categories)
            .WithMany();

        builder
            .HasMany(e => e.TagIds)
            .WithMany();
    }
}
