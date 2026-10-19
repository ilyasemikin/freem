using Freem.Domain;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Relations.Collections.Exceptions;
using Freem.Domain.Storage.Exceptions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations.Extensions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Converters;

internal sealed class TriggerConstraintErrorToExceptionConverter
{
    private readonly EntityIdentifierFactory _identifierFactory;

    public TriggerConstraintErrorToExceptionConverter(EntityIdentifierFactory identifierFactory)
    {
        ArgumentNullException.ThrowIfNull(identifierFactory);
        
        _identifierFactory = identifierFactory;
    }

    public Exception Convert(DatabaseContextWriteContext context, TriggerConstraintError error)
    {
        switch (error.Code)
        {
            case TriggerErrorCodes.ActivitiesTagsDifferentUserIds:
            case TriggerErrorCodes.RecordsTagsDifferentUserIds:
            case TriggerErrorCodes.RunningRecordsTagsDifferentUserIds:
            {
                var identifierValue = error.Parameters[TriggerErrorParameters.TagId].AsString();
                var identifier = _identifierFactory.Create(EntityNames.Tag, identifierValue);
                return new NotFoundRelatedException(context.ProcessedId, identifier);
            }
            case TriggerErrorCodes.ActivitiesTagsInvalidCount:
            case TriggerErrorCodes.RecordsTagsInvalidCount:
            case TriggerErrorCodes.RunningRecordsTagsInvalidCount:
            {
                return new InvalidRelatedCountException(
                    RelatedTagsIdentifiersCollection.MinTagsCount,
                    RelatedTagsIdentifiersCollection.MaxTagsCount,
                    error.Parameters[TriggerErrorParameters.ActualCount].AsInt());
            }
            case TriggerErrorCodes.RecordsActivitiesDifferentUserIds:
            case TriggerErrorCodes.RunningRecordsActivitiesDifferentUserIds:
            {
                var identifierValue = error.Parameters[TriggerErrorParameters.ActivityId].AsString();
                var identifier = _identifierFactory.Create(EntityNames.Activity, identifierValue);
                return new NotFoundRelatedException(context.ProcessedId, identifier);
            }
            case TriggerErrorCodes.RecordsActivitiesInvalidCount:
            case TriggerErrorCodes.RunningRecordsActivitiesInvalidCount:
            {
                return new InvalidRelatedCountException(
                    RelatedActivitiesIdentifiersCollection.MinActivitiesCount,
                    RelatedActivitiesIdentifiersCollection.MaxActivitiesCount,
                    error.Parameters[TriggerErrorParameters.ActualCount].AsInt());
            }
            case TriggerErrorCodes.EventsRelatedEntityNotExists:
            {
                var entityName = error.Parameters[TriggerErrorParameters.EventEntityName].AsString();
                var identifierValue = error.Parameters[TriggerErrorParameters.EventEntityId].AsString();
                var identifier = _identifierFactory.Create(entityName, identifierValue);
                return new NotFoundRelatedException(context.ProcessedId, identifier);
            }
            default:
                throw new InvalidOperationException($"Unknown trigger error code \"{error.Code}\"");
        }
    }
}