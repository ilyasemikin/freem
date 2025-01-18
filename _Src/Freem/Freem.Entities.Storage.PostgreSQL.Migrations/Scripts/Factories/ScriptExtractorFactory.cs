using Freem.Entities.Relations.Collections;
using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Entities.Storage.PostgreSQL.Migrations.Scripts.Constants;
using Freem.Storage.Migrations.Constants.Collections.Builders;
using Freem.Storage.Migrations.Constants.Injection;
using Freem.Storage.Migrations.Scripts;

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Scripts.Factories;

internal static class ScriptExtractorFactory
{
    public static ScriptExtractor Create()
    {
        var builder = new ConstantValuesCollectionBuilder();
            
        builder
            .WithConstant(ConstantNames.SchemaName, EnvironmentNames.Schema);
            
        builder
            .WithConstant(ConstantNames.ActivitiesTagsDifferentUserIds, TriggerErrorCodes.ActivitiesTagsDifferentUserIds)
            .WithConstant(ConstantNames.ActivitiesTagsInvalidCount, TriggerErrorCodes.ActivitiesTagsInvalidCount);
        
        builder
            .WithConstant(ConstantNames.RecordsTagsDifferentUserIds, TriggerErrorCodes.RecordsTagsDifferentUserIds)
            .WithConstant(ConstantNames.RecordsTagsInvalidCount, TriggerErrorCodes.RecordsTagsInvalidCount)
            .WithConstant(ConstantNames.RecordsActivitiesDifferentUserIds, TriggerErrorCodes.RecordsActivitiesDifferentUserIds)
            .WithConstant(ConstantNames.RecordsActivitiesInvalidCount, TriggerErrorCodes.RecordsActivitiesInvalidCount);
            
        builder
            .WithConstant(ConstantNames.RunningRecordsTagsDifferentUserIds, TriggerErrorCodes.RunningRecordsTagsDifferentUserIds)
            .WithConstant(ConstantNames.RunningRecordsTagsInvalidCount, TriggerErrorCodes.RunningRecordsTagsInvalidCount)
            .WithConstant(ConstantNames.RunningRecordsActivitiesDifferentUserIds, TriggerErrorCodes.RunningRecordsActivitiesDifferentUserIds)
            .WithConstant(ConstantNames.RunningRecordsActivitiesInvalidCount, TriggerErrorCodes.RunningRecordsActivitiesInvalidCount);

        builder
            .WithConstant(ConstantNames.EventsRelatedEntityNotExists, TriggerErrorCodes.EventsRelatedEntityNotExists);
        
        builder
            .WithConstant(ConstantNames.EventActivityEntityName, EntitiesNames.Activities.EntityName)
            .WithConstant(ConstantNames.EventRecordEntityName, EntitiesNames.Records.EntityName)
            .WithConstant(ConstantNames.EventRunningRecordEntityName, EntitiesNames.RunningRecords.EntityName)
            .WithConstant(ConstantNames.EventTagEntityName, EntitiesNames.Tags.EntityName)
            .WithConstant(ConstantNames.EventUserEntityName, EntitiesNames.Users.EntityName);
        
        builder
            .WithConstant(ConstantNames.MinRelatedTagsCount, RelatedTagsCollection.MinTagsCount.ToString())
            .WithConstant(ConstantNames.MaxRelatedTagsCount, RelatedTagsCollection.MaxTagsCount.ToString())
            .WithConstant(ConstantNames.MinRelatedActivitiesCount, RelatedActivitiesCollection.MinActivitiesCount.ToString())
            .WithConstant(ConstantNames.MaxRelatedActivitiesCount, RelatedActivitiesCollection.MaxActivitiesCount.ToString());

        builder
            .WithConstant(ConstantNames.ActivityIdTriggerErrorParameterName, TriggerErrorParameters.ActivityId)
            .WithConstant(ConstantNames.RecordIdTriggerErrorParameterName, TriggerErrorParameters.RecordId)
            .WithConstant(ConstantNames.TagIdTriggerErrorParameterName, TriggerErrorParameters.TagId)
            .WithConstant(ConstantNames.UserIdTriggerErrorParameterName, TriggerErrorParameters.UserId)
            .WithConstant(ConstantNames.ActualCountTriggerErrorParameterName, TriggerErrorParameters.ActualCount)
            .WithConstant(ConstantNames.EventEntityNameParameterName, TriggerErrorParameters.EventEntityName)
            .WithConstant(ConstantNames.EventEntityIdParameterName, TriggerErrorParameters.EventEntityId);
        
        var collection = builder.Build();
        
        var injector = new ConstantsInjector(collection);
        return new ScriptExtractor(injector);
    }
}