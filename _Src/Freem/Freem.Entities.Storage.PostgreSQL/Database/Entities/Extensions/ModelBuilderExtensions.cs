using Freem.EFCore.Extensions;
using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Extensions;
using Microsoft.EntityFrameworkCore;
using ActivityStatus = Freem.Entities.Storage.PostgreSQL.Database.Entities.Models.ActivityStatus;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Extensions;

internal static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyEntitiesConfigurations(this ModelBuilder builder)
    {
        builder.ApplyRelationEnttitiesConfiguration();

        builder.HasPostgresEnum<ActivityStatus>(EnvironmentNames.Schema, EntitiesNames.Activities.Models.Status);
        
        builder.ApplyConfiguration<ActivityEntity, ActivityEntityTypeConfiguration>();
        
        builder.ApplyConfiguration<RecordEntity, RecordEntityTypeConfiguration>();
        builder.ApplyConfiguration<RunningRecordEntity, RunningRecordEntityTypeConfiguration>();
        
        builder.ApplyConfiguration<TagEntity, TagEntityTypeConfiguration>();
        
        builder.ApplyConfiguration<UserEntity, UserEntityTypeConfiguration>();
        builder.ApplyConfiguration<UserPasswordCredentialsEntity, UserLoginCredentialsEntityTypeConfiguration>();
        
        builder.ApplyConfiguration<EventEntity, EventEntityTypeConfiguration>();
        
        return builder;
    }
}
