using Freem.Converters.Abstractions;
using Freem.Entities.Abstractions.Relations.Collection.Exceptions;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations.Extensions;
using Freem.Exceptions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Converters;

internal sealed class TriggerConstraintErrorToExceptionConverter : IConverter<TriggerConstraintError, Exception>
{
    public Exception Convert(TriggerConstraintError input)
    {
        switch (input.Code)
        {
            case TriggerErrorCodes.CategoriesTagsDifferentUserIds:
            case TriggerErrorCodes.RecordsTagsDifferentUserIds:
            case TriggerErrorCodes.RunningRecordsTagsDifferentUserIds:
            {
                var identifier = input.Parameters[TriggerErrorParameters.TagId].AsTagIdentifier();
                return new NotFoundRelatedException(identifier);
            }
            case TriggerErrorCodes.CategoriesTagsInvalidCount:
            case TriggerErrorCodes.RecordsTagsInvalidCount:
            case TriggerErrorCodes.RunningRecordsTagsInvalidCount:
            {
                return new InvalidRelatedEntitiesCountException(
                    RelatedTagsCollection.MinTagsCount,
                    RelatedTagsCollection.MaxTagsCount,
                    input.Parameters[TriggerErrorParameters.ActualCount].AsInt());
            }
            case TriggerErrorCodes.RecordsCategoriesDifferentUserIds:
            case TriggerErrorCodes.RunningRecordsCategoriesDifferentUserIds:
            {
                var identifier = input.Parameters[TriggerErrorParameters.CategoryId].AsCategoryIdentifier();
                return new NotFoundRelatedException(identifier);
            }
            case TriggerErrorCodes.RecordsCategoriesInvalidCount:
            case TriggerErrorCodes.RunningRecordsCategoriesInvalidCount:
            {
                return new InvalidRelatedEntitiesCountException(
                    RelatedCategoriesCollection.MinCategoriesCount,
                    RelatedCategoriesCollection.MaxCategoriesCount,
                    input.Parameters[TriggerErrorParameters.ActualCount].AsInt());
            }
            default:
                throw new UnknownConstantException(input.Code);
        }
    }
}