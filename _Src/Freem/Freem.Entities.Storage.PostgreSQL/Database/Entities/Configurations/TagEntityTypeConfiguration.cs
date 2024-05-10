using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;

internal class TagEntityTypeConfiguration : IEntityTypeConfiguration<TagEntity>
{
    public void Configure(EntityTypeBuilder<TagEntity> builder)
    {
        builder.ToTable(Names.Tables.Tags, Names.Schema);

        builder
            .Property(e => e.Id)
            .HasColumnName(Names.Properties.Tags.Id)
            .IsRequired();

        builder
            .Property(e => e.Name)
            .HasColumnName(Names.Properties.Tags.Name)
            .IsRequired();

        builder
            .HasKey(x => x.Id)
            .HasName(Names.Constrains.Tags.PrimaryKey);

        builder
            .HasIndex(e => e.Name)
            .HasDatabaseName(Names.Constrains.Tags.NameIndex)
            .IsUnique()
            .HasDatabaseName(Names.Constrains.Tags.NameUnique);
    }
}
