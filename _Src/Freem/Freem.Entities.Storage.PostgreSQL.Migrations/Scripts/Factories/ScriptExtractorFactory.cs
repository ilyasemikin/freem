using Freem.Entities.Relations.Collections;
using Freem.Entities.Storage.PostgreSQL.Database.Constants;
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
            .WithConstant(ConstantNames.CategoriesTagsDifferentUserIds, TriggerErrorCodes.CategoriesTagsDifferentUserIds)
            .WithConstant(ConstantNames.CategoriesTagsInvalidCount, TriggerErrorCodes.CategoriesTagsInvalidCount);
        
        builder
            .WithConstant(ConstantNames.RecordsTagsDifferentUserIds, TriggerErrorCodes.RecordsTagsDifferentUserIds)
            .WithConstant(ConstantNames.RecordsTagsInvalidCount, TriggerErrorCodes.RecordsTagsInvalidCount)
            .WithConstant(ConstantNames.RecordsCategoriesDifferentUserIds, TriggerErrorCodes.RecordsCategoriesDifferentUserIds)
            .WithConstant(ConstantNames.RecordsCategoriesInvalidCount, TriggerErrorCodes.RecordsCategoriesInvalidCount);
            
        builder
            .WithConstant(ConstantNames.RunningRecordsTagsDifferentUserIds, TriggerErrorCodes.RunningRecordsTagsDifferentUserIds)
            .WithConstant(ConstantNames.RunningRecordsTagsInvalidCount, TriggerErrorCodes.RunningRecordsTagsInvalidCount)
            .WithConstant(ConstantNames.RunningRecordsCategoriesDifferentUserIds, TriggerErrorCodes.RunningRecordsCategoriesDifferentUserIds)
            .WithConstant(ConstantNames.RunningRecordsCategoriesInvalidCount, TriggerErrorCodes.RunningRecordsCategoriesInvalidCount);

        builder
            .WithConstant(ConstantNames.EventsUserNotExist, TriggerErrorCodes.EventsUserNotExist)
            .WithConstant(ConstantNames.CategoriesEventsCategoryNotExist, TriggerErrorCodes.CategoriesEventsCategoryNotExist)
            .WithConstant(ConstantNames.RecordsEventsRecordNotExist, TriggerErrorCodes.RecordsEventsRecordNotExist)
            .WithConstant(ConstantNames.RunningRecordsEventsUserNotExist, TriggerErrorCodes.RunningRecordsEventsUserNotExist)
            .WithConstant(ConstantNames.TagsEventsTagNotExist, TriggerErrorCodes.TagsEventsTagNotExist)
            .WithConstant(ConstantNames.CategoriesEventsDifferentUserIds, TriggerErrorCodes.CategoriesEventsDifferentUserIds)
            .WithConstant(ConstantNames.RecordsEventsDifferentUserIds, TriggerErrorCodes.RecordsEventsDifferentUserIds)
            .WithConstant(ConstantNames.TagsEventsDifferentUserIds, TriggerErrorCodes.TagsEventsDifferentUserIds);
        
        builder
            .WithConstant(ConstantNames.MinRelatedTagsCount, RelatedTagsCollection.MinTagsCount.ToString())
            .WithConstant(ConstantNames.MaxRelatedTagsCount, RelatedTagsCollection.MaxTagsCount.ToString())
            .WithConstant(ConstantNames.MinRelatedCategoriesCount, RelatedCategoriesCollection.MinCategoriesCount.ToString())
            .WithConstant(ConstantNames.MaxRelatedCategoriesCount, RelatedCategoriesCollection.MaxCategoriesCount.ToString());

        builder
            .WithConstant(ConstantNames.CategoryIdTriggerErrorParameterName, TriggerErrorParameters.CategoryId)
            .WithConstant(ConstantNames.RecordIdTriggerErrorParameterName, TriggerErrorParameters.RecordId)
            .WithConstant(ConstantNames.TagIdTriggerErrorParameterName, TriggerErrorParameters.TagId)
            .WithConstant(ConstantNames.UserIdTriggerErrorParameterName, TriggerErrorParameters.UserId)
            .WithConstant(ConstantNames.ActualCountTriggerErrorParameterName, TriggerErrorParameters.ActualCount);
        
        var collection = builder.Build();
        
        var injector = new ConstantsInjector(collection);
        return new ScriptExtractor(injector);
    }
}