using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freem.Entities.Storage.PostgreSQL.Database.Relations.Configurations;

internal class RunningRecordTagRelationEntityTypeConfiguration : IEntityTypeConfiguration<RunningRecordTagRelation>
{
    public void Configure(EntityTypeBuilder<RunningRecordTagRelation> builder)
    {
        builder.ToTable(Names.Relations.Tables.RunningRecordsTags, Names.Schema);

        builder
            .Property(e => e.RunningRecordUserId)
            .HasColumnName(Names.Relations.Properties.RunningRecordsTags.RunningRecordUserId)
            .IsRequired();

        builder
            .Property(e => e.TagId)
            .HasColumnName(Names.Relations.Properties.RunningRecordsTags.TagId)
            .IsRequired();

        builder
            .HasKey(e => new { e.RunningRecordUserId, e.TagId })
            .HasName(Names.Relations.Constrains.RunningRecordsTags.PrimaryKey);

        builder
            .HasIndex(e => e.RunningRecordUserId)
            .HasDatabaseName(Names.Relations.Constrains.RunningRecordsTags.RunningRecordUserIdIndex);
        
        builder
            .HasIndex(e => e.TagId)
            .HasDatabaseName(Names.Relations.Constrains.RunningRecordsTags.TagIdIndex);

        builder
            .HasOne(e => e.RunningRecord)
            .WithMany()
            .HasForeignKey(e => e.RunningRecordUserId)
            .HasConstraintName(Names.Relations.Constrains.RunningRecordsTags.RunningRecordsForeignKey)
            .IsRequired();

        builder
            .HasOne(e => e.Tag)
            .WithMany()
            .HasForeignKey(e => e.TagId)
            .HasConstraintName(Names.Relations.Constrains.RunningRecordsTags.TagsForeignKey)
            .IsRequired();
    }
}