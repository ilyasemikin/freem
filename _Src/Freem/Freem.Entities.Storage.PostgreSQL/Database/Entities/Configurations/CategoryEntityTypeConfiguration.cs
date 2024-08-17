using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.ToTable(EntitiesNames.Categories.Table);

        builder
            .Property(e => e.Id)
            .HasColumnName(EntitiesNames.Categories.Properties.Id)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.UserId)
            .HasColumnName(EntitiesNames.Categories.Properties.UserId)
            .IsRequired();

        builder
            .Property(e => e.Name)
            .HasMaxLength(Category.MaxNameLength)
            .HasColumnName(EntitiesNames.Categories.Properties.Name);

        builder
            .Property(e => e.Status)
            .HasColumnName(EntitiesNames.Categories.Properties.Status)
            .HasColumnType($"{EnvironmentNames.Schema}.{EntitiesNames.Categories.Models.CategoryStatus}")
            .IsRequired();

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(EntitiesNames.Categories.Properties.CreatedAt)
            .HasColumnOrder(1)
            .IsRequired();

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName(EntitiesNames.Categories.Properties.UpdatedAt);

        builder
            .HasKey(e => e.Id)
            .HasName(EntitiesNames.Categories.Constaints.PrimaryKey);

        builder
            .HasIndex(e => e.UserId)
            .HasDatabaseName(EntitiesNames.Categories.Constaints.UserIdIndex);

        builder
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .HasConstraintName(EntitiesNames.Categories.Constaints.UsersForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasMany(e => e.Tags)
            .WithMany(e => e.Categories)
            .UsingEntity<CategoryTagRelationEntity>();
    }
}
