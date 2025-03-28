﻿namespace Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;

internal static class TriggerErrorCodes
{
    public const string ActivitiesTagsDifferentUserIds = "ActivitiesTagsDifferentUserIds";
    public const string ActivitiesTagsInvalidCount = "ActivitiesTagsInvalidCount";

    public const string RecordsTagsDifferentUserIds = "RecordsTagsDifferentUserIds";
    public const string RecordsTagsInvalidCount = "RecordsTagsInvalidCount";
    public const string RecordsActivitiesDifferentUserIds = "RecordsActivitiesDifferentUserIds";
    public const string RecordsActivitiesInvalidCount = "RecordsActivitiesInvalidCount";
    
    public const string RunningRecordsTagsDifferentUserIds = "RunningRecordsTagsDifferentUserIds";
    public const string RunningRecordsTagsInvalidCount = "RunningRecordsTagsInvalidCount";
    public const string RunningRecordsActivitiesDifferentUserIds = "RunningRecordsActivitiesDifferentUserIds";
    public const string RunningRecordsActivitiesInvalidCount = "RunningRecordsActivitiesInvalidCount";
    
    public const string EventsRelatedEntityNotExists = "EventsRelatedEntityNotExists";
}