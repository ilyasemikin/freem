using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Scripts.Constants;

internal static class ConstantNames
{
    public const string SchemaName = $"{nameof(EnvironmentNames)}.{nameof(EnvironmentNames.Schema)}";
    
    public const string CategoriesTagsDifferentUserIds = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.CategoriesTagsDifferentUserIds)}";
    public const string CategoriesTagsInvalidCount = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.CategoriesTagsInvalidCount)}";
    public const string RecordsTagsDifferentUserIds = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RecordsTagsDifferentUserIds)}";
    public const string RecordsTagsInvalidCount = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RecordsTagsInvalidCount)}";
    public const string RecordsCategoriesDifferentUserIds = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RecordsCategoriesDifferentUserIds)}";
    public const string RecordsCategoriesInvalidCount = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RecordsCategoriesInvalidCount)}";
    public const string RunningRecordsTagsDifferentUserIds = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RunningRecordsTagsDifferentUserIds)}";
    public const string RunningRecordsTagsInvalidCount = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RunningRecordsTagsInvalidCount)}";
    public const string RunningRecordsCategoriesDifferentUserIds = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RunningRecordsCategoriesDifferentUserIds)}";
    public const string RunningRecordsCategoriesInvalidCount = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RunningRecordsCategoriesInvalidCount)}";
}