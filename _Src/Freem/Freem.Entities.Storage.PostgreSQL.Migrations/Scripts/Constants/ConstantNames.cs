using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Scripts.Constants;

internal static class ConstantNames
{
    public const string SchemaName = $"{nameof(EnvironmentNames)}.{nameof(EnvironmentNames.Schema)}";
    
    public const string CategoriesTagsDifferentUserIds = $"{nameof(ErrorCodes)}.{nameof(ErrorCodes.CategoriesTagsDifferentUserIds)}";
    public const string CategoriesTagsInvalidCount = $"{nameof(ErrorCodes)}.{nameof(ErrorCodes.CategoriesTagsInvalidCount)}";
    public const string RecordsTagsDifferentUserIds = $"{nameof(ErrorCodes)}.{nameof(ErrorCodes.RecordsTagsDifferentUserIds)}";
    public const string RecordsTagsInvalidCount = $"{nameof(ErrorCodes)}.{nameof(ErrorCodes.RecordsTagsInvalidCount)}";
    public const string RecordsCategoriesDifferentUserIds = $"{nameof(ErrorCodes)}.{nameof(ErrorCodes.RecordsCategoriesDifferentUserIds)}";
    public const string RecordsCategoriesInvalidCount = $"{nameof(ErrorCodes)}.{nameof(ErrorCodes.RecordsCategoriesInvalidCount)}";
    public const string RunningRecordsTagsDifferentUserIds = $"{nameof(ErrorCodes)}.{nameof(ErrorCodes.RunningRecordsTagsDifferentUserIds)}";
    public const string RunningRecordsTagsInvalidCount = $"{nameof(ErrorCodes)}.{nameof(ErrorCodes.RunningRecordsTagsInvalidCount)}";
    public const string RunningRecordsCategoriesDifferentUserIds = $"{nameof(ErrorCodes)}.{nameof(ErrorCodes.RunningRecordsCategoriesDifferentUserIds)}";
    public const string RunningRecordsCategoriesInvalidCount = $"{nameof(ErrorCodes)}.{nameof(ErrorCodes.RunningRecordsCategoriesInvalidCount)}";
}