using Freem.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Database.Relations.Configurations.Extensions;

internal static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyRelationsConfigurations(this ModelBuilder builder) 
    {
        builder.ApplyConfiguration<CategoryTagRelation, CategoryTagRelationEntityTypeConfiguration>();
        builder.ApplyConfiguration<RecordTagRelation, RecordTagRelationEntityTypeConfiguration>();
        builder.ApplyConfiguration<RunningRecordTagRelation, RunningRecordTagRelationEntityTypeConfiguration>();

        builder.ApplyConfiguration<RecordCategoryRelation, RecordCategoryRelationEntityTypeConfiguration>();
        builder.ApplyConfiguration<RunningRecordCategoryRelation, RunningRecordCategoryRelationEntityTypeConfiguration>();

        return builder;
    }
}