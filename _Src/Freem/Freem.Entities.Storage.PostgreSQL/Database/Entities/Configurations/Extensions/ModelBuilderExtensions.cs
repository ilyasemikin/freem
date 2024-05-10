using Freem.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations.Extensions;

internal static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyEntitiesConfigurations(this ModelBuilder builder) 
    {
        builder.ApplyConfiguration<CategoryEntity, CategoryEntityTypeConfiguration>();

        builder.ApplyConfiguration<RecordEntity, RecordEntityTypeConfiguration>();
        builder.ApplyConfiguration<RunningRecordEntity, RunningRecordEntityTypeConfiguration>();
        
        builder.ApplyConfiguration<TagEntity, TagEntityTypeConfiguration>();

        builder.ApplyConfiguration<UserEntity, UserEntityTypeConfiguration>();

        return builder;
    }
}