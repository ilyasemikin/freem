﻿using Freem.Entities.Storage.PostgreSQL.Database.Constants;
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
    public const string EventsUserNotExist = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.EventsUserNotExist)}";
    public const string CategoriesEventsCategoryNotExist = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.CategoriesEventsCategoryNotExist)}";
    public const string RecordsEventsRecordNotExist = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RecordsEventsRecordNotExist)}";
    public const string RunningRecordsEventsUserNotExist = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RunningRecordsEventsUserNotExist)}";
    public const string TagsEventsTagNotExist = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.TagsEventsTagNotExist)}";
    public const string CategoriesEventsDifferentUserIds = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.CategoriesEventsDifferentUserIds)}";
    public const string RecordsEventsDifferentUserIds = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.RecordsEventsDifferentUserIds)}";
    public const string TagsEventsDifferentUserIds = $"{nameof(TriggerErrorCodes)}.{nameof(TriggerErrorCodes.TagsEventsDifferentUserIds)}";

    public const string CategoryIdTriggerErrorParameterName = "TriggerErrorParameters.CategoryIdTriggerErrorParameterName";
    public const string RecordIdTriggerErrorParameterName = "TriggerErrorParameters.RecordIdTriggerErrorParameterName";
    public const string TagIdTriggerErrorParameterName = "TriggerErrorParameters.TagIdTriggerErrorParameterName";
    public const string UserIdTriggerErrorParameterName = "TriggerErrorParameters.UserIdTriggerErrorParameterName";
    public const string ActualCountTriggerErrorParameterName = "TriggerErrorParameters.ActualCountTriggerErrorParameterName";
    
    public const string MinRelatedTagsCount = "Limits.MinRelatedTagsCount";
    public const string MaxRelatedTagsCount = "Limits.MaxRelatedTagsCount";
    public const string MinRelatedCategoriesCount = "Limits.MinRelatedCategoriesCount";
    public const string MaxRelatedCategoriesCount = "Limits.MaxRelatedCategoriesCount";
}