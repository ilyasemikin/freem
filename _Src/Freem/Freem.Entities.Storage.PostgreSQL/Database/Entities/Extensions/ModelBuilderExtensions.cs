using Freem.EFCore.Extensions;
using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations.Events;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Configurations.Events.Base;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Extensions;
using Microsoft.EntityFrameworkCore;

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

        builder.HasPostgresEnum<EventAction>(
            EnvironmentNames.Schema,
            EntitiesNames.Events.Models.Action);
        
        builder.ApplyConfiguration<BaseEventEntity, BaseEventEntityConfiguration>();
        builder.ApplyConfiguration<ActivityEventEntity, ActivityEventEntityConfiguration>();
        builder.ApplyConfiguration<RecordEventEntity, RecordEventEntityConfiguration>();
        builder.ApplyConfiguration<RunningRecordEventEntity, RunningRecordEventEntityConfiguration>();
        builder.ApplyConfiguration<TagEventEntity, TagEventEntityConfiguration>();
        
        return builder;
    }
}
