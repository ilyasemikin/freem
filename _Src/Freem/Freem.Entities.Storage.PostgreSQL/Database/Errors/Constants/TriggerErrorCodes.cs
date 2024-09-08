namespace Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;

internal static class TriggerErrorCodes
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
    
    public const string EventsUserNotExist = "EventsUserNotExists";
    public const string CategoriesEventsCategoryNotExist = "CategoriesEventsCategoryNotExist";
    public const string RecordsEventsRecordNotExist = "RecordsEventsRecordNotExist";
    public const string RunningRecordsEventsUserNotExist = "RunningRecordsEventsUserNotExist";
    public const string TagsEventsTagNotExist = "TagsEventsTagNotExist";
    
    public const string CategoriesEventsDifferentUserIds = "CategoriesEventsDifferentUserIds";
    public const string RecordsEventsDifferentUserIds = "RecordsEventsDifferentUserIds";
    public const string TagsEventsDifferentUserIds = "TagsEventsDifferentUserIds";
}