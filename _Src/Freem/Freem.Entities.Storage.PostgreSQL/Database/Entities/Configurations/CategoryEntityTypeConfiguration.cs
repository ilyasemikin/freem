using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;

internal class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.ToTable(
            Names.Tables.Categories, 
            Names.Schema);

        builder
            .Property(e => e.Id)
            .HasColumnName(Names.Properties.Categories.Id)
            .IsRequired();

        builder
            .Property(e => e.UserId)
            .HasColumnName(Names.Properties.Categories.UserId)
            .IsRequired();

        builder
            .Property(e => e.Name)
            .HasColumnName(Names.Properties.Categories.Name);

        builder
            .HasKey(e => e.Id)
            .HasName(Names.Constrains.Categories.PrimaryKey);

        builder
            .HasIndex(e => e.UserId)
            .HasDatabaseName(Names.Constrains.Categories.UserIdIndex);

        builder
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(Names.Constrains.Categories.UsersForeignKey)
            .IsRequired();

        builder
            .HasMany(e => e.Tags)
            .WithMany()
            .UsingEntity<CategoryTagRelation>();
    }
}
