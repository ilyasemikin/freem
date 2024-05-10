using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Relations.Configurations;

internal class RecordTagRelationEntityTypeConfiguration : IEntityTypeConfiguration<RecordTagRelation>
{
    public void Configure(EntityTypeBuilder<RecordTagRelation> builder)
    {
        builder.ToTable(Names.Relations.Tables.RecordsTags, Names.Schema);

        builder
            .Property(e => e.RecordId)
            .HasColumnName(Names.Relations.Properties.RecordsTags.RecordId)
            .IsRequired();

        builder
            .Property(e => e.TagId)
            .HasColumnName(Names.Relations.Properties.RecordsTags.TagId)
            .IsRequired();

        builder
            .HasKey(e => new { e.RecordId, e.TagId })
            .HasName(Names.Relations.Constrains.RecordsTags.PrimaryKey);

        builder
            .HasIndex(e => e.RecordId)
            .HasDatabaseName(Names.Relations.Constrains.RecordsTags.RecordIdIndex);

        builder
            .HasIndex(e => e.TagId)
            .HasDatabaseName(Names.Relations.Constrains.RecordsTags.TagIdIndex);

        builder
            .HasOne(e => e.Record)
            .WithMany()
            .HasForeignKey(e => e.RecordId)
            .HasConstraintName(Names.Relations.Constrains.RecordsTags.RecordsForeignKey)
            .IsRequired();

        builder
            .HasOne(e => e.Tag)
            .WithMany()
            .HasForeignKey(e => e.TagId)
            .HasConstraintName(Names.Relations.Constrains.RecordsTags.TagsForeignKey)
            .IsRequired();
    }
}