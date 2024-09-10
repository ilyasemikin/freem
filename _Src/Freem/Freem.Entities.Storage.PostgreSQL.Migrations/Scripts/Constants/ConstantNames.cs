using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Scripts.Constants;

internal static class ConstantNames
{
    public const string SchemaName = $"{nameof(EnvironmentNames)}.{nameof(EnvironmentNames.Schema)}";
    
    public const string ActivitiesTagsDifferentUserIds = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.ActivitiesTagsDifferentUserIds)}";
    public const string ActivitiesTagsInvalidCount = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.ActivitiesTagsInvalidCount)}";
    public const string RecordsTagsDifferentUserIds = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RecordsTagsDifferentUserIds)}";
    public const string RecordsTagsInvalidCount = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RecordsTagsInvalidCount)}";
    public const string RecordsActivitiesDifferentUserIds = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RecordsActivitiesDifferentUserIds)}";
    public const string RecordsActivitiesInvalidCount = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RecordsActivitiesInvalidCount)}";
    public const string RunningRecordsTagsDifferentUserIds = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RunningRecordsTagsDifferentUserIds)}";
    public const string RunningRecordsTagsInvalidCount = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RunningRecordsTagsInvalidCount)}";
    public const string RunningRecordsActivitiesDifferentUserIds = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RunningRecordsActivitiesDifferentUserIds)}";
    public const string RunningRecordsActivitiesInvalidCount = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RunningRecordsActivitiesInvalidCount)}";
    public const string EventsUserNotExist = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.EventsUserNotExist)}";
    public const string ActivitiesEventsActivityNotExist = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.ActivitiesEventsActivityNotExist)}";
    public const string RecordsEventsRecordNotExist = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RecordsEventsRecordNotExist)}";
    public const string RunningRecordsEventsUserNotExist = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RunningRecordsEventsUserNotExist)}";
    public const string TagsEventsTagNotExist = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.TagsEventsTagNotExist)}";
    public const string ActivitiesEventsDifferentUserIds = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.ActivitiesEventsDifferentUserIds)}";
    public const string RecordsEventsDifferentUserIds = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RecordsEventsDifferentUserIds)}";
    public const string TagsEventsDifferentUserIds = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.TagsEventsDifferentUserIds)}";

    public const string ActivityIdTriggerErrorParameterName = "TriggerErrorParameters.ActivityIdTriggerErrorParameterName";
    public const string RecordIdTriggerErrorParameterName = "TriggerErrorParameters.RecordIdTriggerErrorParameterName";
    public const string TagIdTriggerErrorParameterName = "TriggerErrorParameters.TagIdTriggerErrorParameterName";
    public const string UserIdTriggerErrorParameterName = "TriggerErrorParameters.UserIdTriggerErrorParameterName";
    public const string ActualCountTriggerErrorParameterName = "TriggerErrorParameters.ActualCountTriggerErrorParameterName";
    
    public const string MinRelatedTagsCount = "Limits.MinRelatedTagsCount";
    public const string MaxRelatedTagsCount = "Limits.MaxRelatedTagsCount";
    public const string MinRelatedActivitiesCount = "Limits.MinRelatedActivitiesCount";
    public const string MaxRelatedActivitiesCount = "Limits.MaxRelatedActivitiesCount";
}