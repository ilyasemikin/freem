﻿using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;

internal sealed class RunningRecordEntityTypeConfiguration : IEntityTypeConfiguration<RunningRecordEntity>
{
    public void Configure(EntityTypeBuilder<RunningRecordEntity> builder)
    {
        builder.ToTable(EntitiesNames.RunningRecords.Table);

        builder
            .Property(e => e.UserId)
            .HasColumnName(EntitiesNames.RunningRecords.Properties.UserId)
            .HasColumnOrder(0)
            .IsRequired();

        builder
            .Property(e => e.Name)
            .HasMaxLength(RunningRecord.MaxNameLength)
            .HasColumnName(EntitiesNames.RunningRecords.Properties.Name);

        builder
            .Property(e => e.Description)
            .HasMaxLength(RunningRecord.MaxDescriptionLength)
            .HasColumnName(EntitiesNames.RunningRecords.Properties.Description);

        builder
            .Property(e => e.StartAt)
            .HasColumnName(EntitiesNames.RunningRecords.Properties.StartAt)
            .IsRequired();

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(EntitiesNames.RunningRecords.Properties.CreatedAt)
            .HasColumnOrder(1)
            .IsRequired();

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName(EntitiesNames.RunningRecords.Properties.UpdatedAt);

        builder
            .HasKey(e => e.UserId)
            .HasName(EntitiesNames.RunningRecords.Constaints.PrimaryKey);

        builder
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .HasConstraintName(EntitiesNames.RunningRecords.Constaints.UsersForeignKey)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasMany(e => e.Categories)
            .WithMany()
            .UsingEntity<RunningRecordCategoryRelationEntity>();

        builder
            .HasMany(e => e.Tags)
            .WithMany()
            .UsingEntity<RunningRecordTagRelationEntity>();
    }
}
