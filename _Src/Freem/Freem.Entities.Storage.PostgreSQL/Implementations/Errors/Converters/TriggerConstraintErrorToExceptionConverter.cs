using Freem.Converters.Abstractions;
using Freem.Entities.Abstractions.Relations.Collection.Exceptions;
using Freem.Entities.Activities;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations.Extensions;
using Freem.Entities.Tags;
using Freem.Exceptions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Converters;

internal sealed class TriggerConstraintErrorToExceptionConverter :
    IConverter<DatabaseContextWriteContext, TriggerConstraintError, Exception>
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
                var identifier = _identifierFactory.Create(Tag.EntityName, identifierValue);
                return new NotFoundRelatedException(context.ProcessedId, identifier);
            }
            case TriggerErrorCodes.ActivitiesTagsInvalidCount:
            case TriggerErrorCodes.RecordsTagsInvalidCount:
            case TriggerErrorCodes.RunningRecordsTagsInvalidCount:
            {
                return new InvalidRelatedEntitiesCountException(
                    RelatedTagsCollection.MinTagsCount,
                    RelatedTagsCollection.MaxTagsCount,
                    error.Parameters[TriggerErrorParameters.ActualCount].AsInt());
            }
            case TriggerErrorCodes.RecordsActivitiesDifferentUserIds:
            case TriggerErrorCodes.RunningRecordsActivitiesDifferentUserIds:
            {
                var identifierValue = error.Parameters[TriggerErrorParameters.ActivityId].AsString();
                var identifier = _identifierFactory.Create(Activity.EntityName, identifierValue);
                return new NotFoundRelatedException(context.ProcessedId, identifier);
            }
            case TriggerErrorCodes.RecordsActivitiesInvalidCount:
            case TriggerErrorCodes.RunningRecordsActivitiesInvalidCount:
            {
                return new InvalidRelatedEntitiesCountException(
                    RelatedActivitiesCollection.MinActivitiesCount,
                    RelatedActivitiesCollection.MaxActivitiesCount,
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
                throw new UnknownConstantException(error.Code);
        }
    }
}