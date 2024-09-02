namespace Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;

internal static class ErrorCodes
{
    public const string CategoriesTagsDifferentUserIds = "CategoriesTagsDifferentUserIds";
    public const string CategoriesTagsInvalidCount = "CategoriesTagsInvalidCount";

    public const string RecordsTagsDifferentUserIds = "RecordsTagsDifferentUserIds";
    public const string RecordsTagsInvalidCount = "RecordsTagsInvalidCount";
    public const string RecordsCategoriesDifferentUserIds = "RecordsCategoriesDifferentUserIds";
    public const string RecordsCategoriesInvalidCount = "RecordsCategoriesInvalidCount";
    
    public const string RunningRecordsTagsDifferentUserIds = "RunningRecordsTagsDifferentUserIds";
    public const string RunningRecordsTagsInvalidCount = "RunningRecordsTagsInvalidCount";
    public const string RunningRecordsCategoriesDifferentUserIds = "RunningRecordsCategoriesDifferentUserIds";
    public const string RunningRecordsCategoriesInvalidCount = "RunningRecordsCategoriesInvalidCount";
}