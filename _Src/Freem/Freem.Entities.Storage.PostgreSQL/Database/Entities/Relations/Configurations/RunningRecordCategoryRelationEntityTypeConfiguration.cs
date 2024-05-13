﻿using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Configurations;

internal sealed class RunningRecordCategoryRelationEntityTypeConfiguration : IEntityTypeConfiguration<RunningRecordCategoryRelationEntity>
{
    public void Configure(EntityTypeBuilder<RunningRecordCategoryRelationEntity> builder)
    {
        builder.ToTable(RelationsNames.RunningRecordsCategories.Table);

        builder
            .Property(e => e.RunningRecordId)
            .HasColumnName(RelationsNames.RunningRecordsCategories.Properties.RunningRecordId)
            .IsRequired();

        builder
            .Property(e => e.CategoryId)
            .HasColumnName(RelationsNames.RunningRecordsCategories.Properties.CategoryId)
            .IsRequired();

        builder
            .HasKey(e => new { e.RunningRecordId, e.CategoryId })
            .HasName(RelationsNames.RunningRecordsCategories.Constraints.PrimaryKey);

        builder
            .HasIndex(e => e.CategoryId)
            .HasDatabaseName(RelationsNames.RunningRecordsCategories.Constraints.CategoryIdIndex);

        builder
            .HasOne(e => e.RunningRecord)
            .WithMany()
            .HasForeignKey(e => e.RunningRecordId)
            .HasConstraintName(RelationsNames.RunningRecordsCategories.Constraints.RunningRecordsForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .HasConstraintName(RelationsNames.RunningRecordsCategories.Constraints.CategoriesForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
