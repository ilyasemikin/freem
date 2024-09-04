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
        var constants = new ConstantValuesCollectionBuilder()
            .WithConstant(ConstantNames.SchemaName, EnvironmentNames.Schema)
            .WithConstant(ConstantNames.CategoriesTagsDifferentUserIds, TriggerErrorCodes.CategoriesTagsDifferentUserIds)
            .WithConstant(ConstantNames.CategoriesTagsInvalidCount, TriggerErrorCodes.CategoriesTagsInvalidCount)
            .WithConstant(ConstantNames.RecordsTagsDifferentUserIds, TriggerErrorCodes.RecordsTagsDifferentUserIds)
            .WithConstant(ConstantNames.RecordsTagsInvalidCount, TriggerErrorCodes.RecordsTagsInvalidCount)
            .WithConstant(ConstantNames.RecordsCategoriesDifferentUserIds, TriggerErrorCodes.RecordsCategoriesDifferentUserIds)
            .WithConstant(ConstantNames.RecordsCategoriesInvalidCount, TriggerErrorCodes.RecordsCategoriesInvalidCount)
            .WithConstant(ConstantNames.RunningRecordsTagsDifferentUserIds, TriggerErrorCodes.RunningRecordsTagsDifferentUserIds)
            .WithConstant(ConstantNames.RunningRecordsTagsInvalidCount, TriggerErrorCodes.RunningRecordsTagsInvalidCount)
            .WithConstant(ConstantNames.RunningRecordsCategoriesDifferentUserIds, TriggerErrorCodes.RunningRecordsCategoriesDifferentUserIds)
            .WithConstant(ConstantNames.RunningRecordsCategoriesInvalidCount, TriggerErrorCodes.RunningRecordsCategoriesInvalidCount)
            .Build();
        
        var injector = new ConstantsInjector(constants);
        return new ScriptExtractor(injector);
    }
}