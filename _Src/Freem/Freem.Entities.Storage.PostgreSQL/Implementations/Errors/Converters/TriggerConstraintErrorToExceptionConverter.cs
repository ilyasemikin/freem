using Freem.Converters.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Abstractions.Relations.Collection.Exceptions;
using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations.Extensions;
using Freem.Exceptions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Converters;

internal sealed class TriggerConstraintErrorToExceptionConverter : 
    IConverter<DatabaseContextWriteContext, TriggerConstraintError, Exception>
{
    public Exception Convert(DatabaseContextWriteContext context, TriggerConstraintError error)
    {
        switch (error.Code)
        {
            case TriggerErrorCodes.ActivitiesTagsDifferentUserIds:
            case TriggerErrorCodes.RecordsTagsDifferentUserIds:
            case TriggerErrorCodes.RunningRecordsTagsDifferentUserIds:
            {
                var identifier = error.Parameters[TriggerErrorParameters.TagId].AsTagIdentifier();
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
                var identifier = error.Parameters[TriggerErrorParameters.ActivityId].AsActivityIdentifier();
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
                var identifierParameter = error.Parameters[TriggerErrorParameters.EventEntityId];

                IEntityIdentifier identifier = entityName switch
                {
                    EntitiesNames.Activities.EntityName => identifierParameter.AsActivityIdentifier(),
                    EntitiesNames.Records.EntityName => identifierParameter.AsRecordIdentifier(),
                    EntitiesNames.RunningRecords.EntityName => identifierParameter.AsRunningRecordIdentifier(),
                    EntitiesNames.Tags.EntityName => identifierParameter.AsTagIdentifier(),
                    EntitiesNames.Users.EntityName => identifierParameter.AsUserIdentifier(),
                    EntitiesNames.UsersLoginCredentials.EntityName => identifierParameter.AsUserIdentifier(),
                    EntitiesNames.UserTelegramIntegration.EntityName => identifierParameter.AsUserIdentifier(),
                    _ => throw new InvalidOperationException($"Unknown entity name \"{entityName}\"")
                };
                
                return new NotFoundRelatedException(context.ProcessedId, identifier);
            }
            default:
                throw new UnknownConstantException(error.Code);
        }
    }
}