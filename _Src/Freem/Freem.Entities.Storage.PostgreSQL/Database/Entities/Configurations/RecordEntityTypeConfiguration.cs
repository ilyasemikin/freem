using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;

internal class RecordEntityTypeConfiguration : IEntityTypeConfiguration<RecordEntity>
{
    public void Configure(EntityTypeBuilder<RecordEntity> builder)
    {
        builder
            .Property(e => e.Id)
            .IsRequired();

        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Name);

        builder
            .Property(e => e.Description);

        builder
            .Property(e => e.StartAt)
            .IsRequired();

        builder
            .Property(e => e.EndAt)
            .IsRequired();

        builder
            .HasMany(e => e.Categories)
            .WithMany();

        builder
            .HasMany(e => e.Tags)
            .WithMany();
    }
}
