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
            .WithConstant(ConstantNames.CategoriesTagsDifferentUserIds, ErrorCodes.CategoriesTagsDifferentUserIds)
            .WithConstant(ConstantNames.CategoriesTagsInvalidCount, ErrorCodes.CategoriesTagsInvalidCount)
            .WithConstant(ConstantNames.RecordsTagsDifferentUserIds, ErrorCodes.RecordsTagsDifferentUserIds)
            .WithConstant(ConstantNames.RecordsTagsInvalidCount, ErrorCodes.RecordsTagsInvalidCount)
            .WithConstant(ConstantNames.RecordsCategoriesDifferentUserIds, ErrorCodes.RecordsCategoriesDifferentUserIds)
            .WithConstant(ConstantNames.RecordsCategoriesInvalidCount, ErrorCodes.RecordsCategoriesInvalidCount)
            .WithConstant(ConstantNames.RunningRecordsTagsDifferentUserIds, ErrorCodes.RunningRecordsTagsDifferentUserIds)
            .WithConstant(ConstantNames.RunningRecordsTagsInvalidCount, ErrorCodes.RunningRecordsTagsInvalidCount)
            .WithConstant(ConstantNames.RunningRecordsCategoriesDifferentUserIds, ErrorCodes.RunningRecordsCategoriesDifferentUserIds)
            .WithConstant(ConstantNames.RunningRecordsCategoriesInvalidCount, ErrorCodes.RunningRecordsCategoriesInvalidCount)
            .Build();
        
        var injector = new ConstantsInjector(constants);
        return new ScriptExtractor(injector);
    }
}