using Freem.Domain.Storage.PostgreSQL.Database.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Configurations;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Relations.Extensions;
using Microsoft.EntityFrameworkCore;
using ActivityStatus = Freem.Domain.Storage.PostgreSQL.Database.Entities.Models.ActivityStatus;
using WeekStart = Freem.Domain.Storage.PostgreSQL.Database.Entities.Models.WeekStart;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Extensions;

internal static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyEntitiesConfigurations(this ModelBuilder builder)
    {
        builder.ApplyRelationEnttitiesConfiguration();

        builder.HasPostgresEnum<ActivityStatus>(EnvironmentNames.Schema, EntitiesNames.Activities.Models.Status);
        builder.HasPostgresEnum<WeekStart>(EnvironmentNames.Schema, EntitiesNames.UserSettings.Models.WeekStart);
        
        builder.ApplyConfiguration(new ActivityEntityTypeConfiguration());
        
        builder.ApplyConfiguration(new RecordEntityTypeConfiguration());
        builder.ApplyConfiguration(new RecordPeriodEntityTypeConfiguration());
        builder.ApplyConfiguration(new RunningRecordEntityTypeConfiguration());
        
        builder.ApplyConfiguration(new TagEntityTypeConfiguration());
        
        builder.ApplyConfiguration(new UserEntityTypeConfiguration());
        builder.ApplyConfiguration(new UserSettingsEntityTypeConfiguration());
        
        builder.ApplyConfiguration(new ImportDocumentEntityTypeConfiguration());
        builder.ApplyConfiguration(new ImportDocumentTagEntityTypeConfiguration());
        builder.ApplyConfiguration(new ImportDocumentActivityEntityTypeConfiguration());
        builder.ApplyConfiguration(new ImportDocumentActivityTagEntityTypeConfiguration());
        builder.ApplyConfiguration(new ImportRowEntityTypeConfiguration());
        builder.ApplyConfiguration(new ImportRowPeriodEntityTypeConfiguration());
        builder.ApplyConfiguration(new ImportRowActivityEntityTypeConfiguration());
        builder.ApplyConfiguration(new ImportRowTagEntityTypeConfiguration());
        
        builder.ApplyConfiguration(new EventEntityTypeConfiguration());
        
        return builder;
    }
}
