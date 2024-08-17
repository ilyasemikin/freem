﻿using Freem.EFCore.Extensions;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Extensions;

internal static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyRelationEnttitiesConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration<CategoryTagRelationEntity, CategoryTagRelationEntityTypeConfiguration>();
        builder.ApplyConfiguration<RecordTagRelationEntity, RecordTagRelationEntityTypeConfiguration>();
        builder.ApplyConfiguration<RunningRecordTagRelationEntity, RunningRecordTagRelationEntityTypeConfiguration>();

        builder.ApplyConfiguration<RecordCategoryRelationEntity, RecordCategoryRelationEntityTypeConfiguration>();
        builder.ApplyConfiguration<RunningRecordCategoryRelationEntity, RunningRecordCategoryRelationEntityTypeConfiguration>();

        return builder;
    }
}
