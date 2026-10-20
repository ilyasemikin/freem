using Freem.Domain.Storage.PostgreSQL.Database.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Configurations;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Relations.Extensions;
using Microsoft.EntityFrameworkCore;
using ActivityStatus = Freem.Domain.Storage.PostgreSQL.Database.Entities.Models.ActivityStatus;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Extensions;

internal static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyEntitiesConfigurations(this ModelBuilder builder)
    {
        builder.ApplyRelationEnttitiesConfiguration();

        builder.HasPostgresEnum<ActivityStatus>(EnvironmentNames.Schema, EntitiesNames.Activities.Models.Status);
        
        builder.ApplyConfiguration(new ActivityEntityTypeConfiguration());
        
        builder.ApplyConfiguration(new RecordEntityTypeConfiguration());
        builder.ApplyConfiguration(new RunningRecordEntityTypeConfiguration());
        
        builder.ApplyConfiguration(new TagEntityTypeConfiguration());
        
        builder.ApplyConfiguration(new UserEntityTypeConfiguration());
        builder.ApplyConfiguration(new UserSettingsEntityTypeConfiguration());
        
        builder.ApplyConfiguration(new EventEntityTypeConfiguration());
        
        return builder;
    }
}
