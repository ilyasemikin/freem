using Freem.Domain.Activities;
using Freem.Domain.Events;
using Freem.Domain.Records;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Relations.Collections.Exceptions;
using Freem.Domain.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Domain.Storage.PostgreSQL.Database.Errors.Implementations.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Events.Converters;
using Freem.Domain.RunningRecords;
using Freem.Domain.Tags;
using Freem.Domain.Users;
using Utilities.Errors;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Errors.Converters;

internal sealed class TriggerConstraintErrorToErrorConverter
{
    public Error Convert(TriggerConstraintError error)
    {
        switch (error.Code)
        {
            case TriggerErrorCodes.ActivitiesTagsDifferentUserIds:
            case TriggerErrorCodes.RecordsTagsDifferentUserIds:
            case TriggerErrorCodes.RunningRecordsTagsDifferentUserIds:
            {
                var idValue = error.Parameters[TriggerErrorParameters.TagId].AsString();
                var tagId = TagId.Parse(idValue, null);
                var tagNotFound = DomainErrorCatalog.Factories.CreateTagNotFound(tagId);
                return DomainErrorCatalog.Factories.CreateRelatedEntitiesNotFound([tagNotFound]);
            }
            case TriggerErrorCodes.ActivitiesTagsInvalidCount:
            case TriggerErrorCodes.RecordsTagsInvalidCount:
            case TriggerErrorCodes.RunningRecordsTagsInvalidCount:
            {
                throw new InvalidRelatedCountException(
                    RelatedTagIdsCollection.MinTagsCount,
                    RelatedTagIdsCollection.MaxTagsCount,
                    error.Parameters[TriggerErrorParameters.ActualCount].AsInt());
            }
            case TriggerErrorCodes.RecordsActivitiesDifferentUserIds:
            case TriggerErrorCodes.RunningRecordsActivitiesDifferentUserIds:
            {
                var idValue = error.Parameters[TriggerErrorParameters.ActivityId].AsString();
                var activityId = ActivityId.Parse(idValue, null);
                var activityNotFound = DomainErrorCatalog.Factories.CreateActivityNotFound(activityId);
                return DomainErrorCatalog.Factories.CreateRelatedEntitiesNotFound([activityNotFound]);
            }
            case TriggerErrorCodes.RecordsActivitiesInvalidCount:
            case TriggerErrorCodes.RunningRecordsActivitiesInvalidCount:
            {
                throw new InvalidRelatedCountException(
                    RelatedActivityIdsCollection.MinActivitiesCount,
                    RelatedActivityIdsCollection.MaxActivitiesCount,
                    error.Parameters[TriggerErrorParameters.ActualCount].AsInt());
            }
            case TriggerErrorCodes.EventsRelatedEntityNotExists:
            {
                var entityName = error.Parameters[TriggerErrorParameters.EventEntityName].AsString();
                var idValue = error.Parameters[TriggerErrorParameters.EventEntityId].AsString();
                var entityType = EntityTypeNames.Parse(entityName);
                var notFound = CreateEntityNotFound(entityType, idValue);
                return DomainErrorCatalog.Factories.CreateRelatedEntitiesNotFound([notFound]);
            }
            default:
                throw new InvalidOperationException($"Unknown trigger error code \"{error.Code}\"");
        }
    }

    private static Error CreateEntityNotFound(EntityType type, string idValue)
    {
        switch (type)
        {
            case EntityType.Activity:
            {
                var id = ActivityId.Parse(idValue, null);
                return DomainErrorCatalog.Factories.CreateActivityNotFound(id);
            }
            case EntityType.Record:
            {
                var id = RecordId.Parse(idValue, null);
                return DomainErrorCatalog.Factories.CreateRecordNotFound(id);
            }
            case EntityType.RunningRecord:
            {
                var id = RunningRecordId.Parse(idValue, null);
                return DomainErrorCatalog.Factories.CreateRunningRecordNotFound(id);
            }
            case EntityType.Tag:
            {
                var id = TagId.Parse(idValue, null);
                return DomainErrorCatalog.Factories.CreateTagNotFound(id);
            }
            case EntityType.User:
            {
                var id = UserId.Parse(idValue, null);
                return DomainErrorCatalog.Factories.CreateUserNotFound(id);
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
