using Freem.Domain;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Relations.Collections.Exceptions;
using Freem.Domain.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Domain.Storage.PostgreSQL.Database.Errors.Implementations.Extensions;
using Utilities.Errors;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Errors.Converters;

internal sealed class TriggerConstraintErrorToErrorConverter
{
    private readonly EntityIdFactory _idFactory;

    public TriggerConstraintErrorToErrorConverter(EntityIdFactory idFactory)
    {
        ArgumentNullException.ThrowIfNull(idFactory);
        
        _idFactory = idFactory;
    }

    public Error Convert(TriggerConstraintError error)
    {
        switch (error.Code)
        {
            case TriggerErrorCodes.ActivitiesTagsDifferentUserIds:
            case TriggerErrorCodes.RecordsTagsDifferentUserIds:
            case TriggerErrorCodes.RunningRecordsTagsDifferentUserIds:
            {
                var idValue = error.Parameters[TriggerErrorParameters.TagId].AsString();
                var id = _idFactory.Create(EntityNames.Tag, idValue);
                return DomainErrorCatalog.Factories.CreateRelatedEntitiesNotFound([id]);
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
                var id = _idFactory.Create(EntityNames.Activity, idValue);
                return DomainErrorCatalog.Factories.CreateRelatedEntitiesNotFound([id]);
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
                var id = _idFactory.Create(entityName, idValue);
                return DomainErrorCatalog.Factories.CreateRelatedEntitiesNotFound([id]);
            }
            default:
                throw new InvalidOperationException($"Unknown trigger error code \"{error.Code}\"");
        }
    }
}