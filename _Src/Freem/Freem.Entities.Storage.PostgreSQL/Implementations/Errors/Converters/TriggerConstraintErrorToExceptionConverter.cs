using Freem.Converters.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Abstractions.Relations.Collection.Exceptions;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Exceptions;
using Freem.Exceptions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Converters;

internal sealed class TriggerConstraintErrorToExceptionConverter : IConverter<TriggerConstraintError, Exception>
{
    public Exception Convert(TriggerConstraintError input)
    {
        switch (input.Code)
        {
            case TriggerErrorCodes.ActivitiesTagsDifferentUserIds:
            case TriggerErrorCodes.RecordsTagsDifferentUserIds:
            case TriggerErrorCodes.RunningRecordsTagsDifferentUserIds:
            {
                var identifier = input.Parameters[TriggerErrorParameters.TagId].AsTagIdentifier();
                return new NotFoundRelatedException(identifier);
            }
            case TriggerErrorCodes.ActivitiesTagsInvalidCount:
            case TriggerErrorCodes.RecordsTagsInvalidCount:
            case TriggerErrorCodes.RunningRecordsTagsInvalidCount:
            {
                return new InvalidRelatedEntitiesCountException(
                    RelatedTagsCollection.MinTagsCount,
                    RelatedTagsCollection.MaxTagsCount,
                    input.Parameters[TriggerErrorParameters.ActualCount].AsInt());
            }
            case TriggerErrorCodes.RecordsActivitiesDifferentUserIds:
            case TriggerErrorCodes.RunningRecordsActivitiesDifferentUserIds:
            {
                var identifier = input.Parameters[TriggerErrorParameters.ActivityId].AsActivityIdentifier();
                return new NotFoundRelatedException(identifier);
            }
            case TriggerErrorCodes.RecordsActivitiesInvalidCount:
            case TriggerErrorCodes.RunningRecordsActivitiesInvalidCount:
            {
                return new InvalidRelatedEntitiesCountException(
                    RelatedActivitiesCollection.MinActivitiesCount,
                    RelatedActivitiesCollection.MaxActivitiesCount,
                    input.Parameters[TriggerErrorParameters.ActualCount].AsInt());
            }
            case TriggerErrorCodes.EventsUserNotExist:
            case TriggerErrorCodes.ActivitiesEventsActivityNotExist:
            case TriggerErrorCodes.RecordsEventsRecordNotExist:
            case TriggerErrorCodes.RunningRecordsEventsUserNotExist:
            case TriggerErrorCodes.TagsEventsTagNotExist:
            {
                var message = GetEventRelatedEntityNotExist(input);
                return new InternalStorageException(message);
            }
            case TriggerErrorCodes.ActivitiesEventsDifferentUserIds:
            case TriggerErrorCodes.RecordsEventsDifferentUserIds:
            case TriggerErrorCodes.TagsEventsDifferentUserIds:
            {
                var message = GetEventDifferentUserIdsExceptionMessage(input);
                return new InternalStorageException(message);
            }
            default:
                throw new UnknownConstantException(input.Code);
        }
    }

    private static string GetEventRelatedEntityNotExist(TriggerConstraintError error)
    {
        var identifier = GetIdentifier(error);
        var identifierName = identifier.GetType().Name;

        return $"Related entity not exists for event. {identifierName} = {identifier}";
    }
    
    private static string GetEventDifferentUserIdsExceptionMessage(TriggerConstraintError error)
    {
        var identifier = GetIdentifier(error);
        var identifierName = identifier.GetType().Name;

        return $"Different user ids, entity and event. {identifierName} = {identifier}";
    }

    private static IEntityIdentifier GetIdentifier(TriggerConstraintError error)
    {
        switch (error.Code)
        {
            case TriggerErrorCodes.ActivitiesEventsActivityNotExist:
            case TriggerErrorCodes.ActivitiesEventsDifferentUserIds:
                return error.Parameters[TriggerErrorParameters.ActivityId].AsActivityIdentifier();
            case TriggerErrorCodes.RecordsEventsRecordNotExist:
            case TriggerErrorCodes.RecordsEventsDifferentUserIds:
                return error.Parameters[TriggerErrorParameters.RecordId].AsRecordIdentifier();
            case TriggerErrorCodes.EventsUserNotExist:
            case TriggerErrorCodes.RunningRecordsEventsUserNotExist:
                return error.Parameters[TriggerErrorParameters.UserId].AsUserIdentifier();
            case TriggerErrorCodes.TagsEventsTagNotExist:
            case TriggerErrorCodes.TagsEventsDifferentUserIds:
                return error.Parameters[TriggerErrorParameters.TagId].AsTagIdentifier();
            default:
                throw new UnknownConstantException(error.Code);
        }
    }
}