using Freem.Domain;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Storage.PostgreSQL.Database.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Domain.Storage.PostgreSQL.Migrations.Constants.Collections.Builders;
using Freem.Domain.Storage.PostgreSQL.Migrations.Constants.Injection;
using Freem.Domain.Storage.PostgreSQL.Migrations.Scripts.Constants;

namespace Freem.Domain.Storage.PostgreSQL.Migrations.Scripts.Factories;

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
            .WithConstant(ConstantNames.EventActivityEntityName, EntityNames.Activity)
            .WithConstant(ConstantNames.EventRecordEntityName, EntityNames.Record)
            .WithConstant(ConstantNames.EventRunningRecordEntityName, EntityNames.RunningRecord)
            .WithConstant(ConstantNames.EventTagEntityName, EntityNames.Tag)
            .WithConstant(ConstantNames.EventUserEntityName, EntityNames.User);
        
        builder
            .WithConstant(ConstantNames.MinRelatedTagsCount, RelatedTagIdsCollection.MinTagsCount.ToString())
            .WithConstant(ConstantNames.MaxRelatedTagsCount, RelatedTagIdsCollection.MaxTagsCount.ToString())
            .WithConstant(ConstantNames.MinRelatedActivitiesCount, RelatedActivityIdsCollection.MinActivitiesCount.ToString())
            .WithConstant(ConstantNames.MaxRelatedActivitiesCount, RelatedActivityIdsCollection.MaxActivitiesCount.ToString());

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