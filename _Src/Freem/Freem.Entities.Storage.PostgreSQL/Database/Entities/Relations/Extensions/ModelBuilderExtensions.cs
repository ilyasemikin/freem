using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Extensions;

internal static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyRelationEnttitiesConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ActivityTagRelationEntityTypeConfiguration());
        builder.ApplyConfiguration(new RecordTagRelationEntityTypeConfiguration());
        builder.ApplyConfiguration(new RunningRecordTagRelationEntityTypeConfiguration());

        builder.ApplyConfiguration(new RecordActivityRelationEntityTypeConfiguration());
        builder.ApplyConfiguration(new RunningRecordActivityRelationEntityTypeConfiguration());

        return builder;
    }
}
