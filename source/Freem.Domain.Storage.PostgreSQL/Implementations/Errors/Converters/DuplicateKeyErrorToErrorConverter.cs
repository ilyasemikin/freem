using System.Diagnostics.CodeAnalysis;
using Freem.Domain.RunningRecords;
using Freem.Domain.Tags;
using Freem.Domain.Users;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Errors.Implementations;
using Utilities.Errors;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Errors.Converters;

internal sealed class DuplicateKeyErrorToErrorConverter
{
    public bool TryConvert(
        DatabaseContextWriteContext context,
        DuplicateKeyError error,
        [NotNullWhen(true)] out Error? output)
    {
        switch (error.ConstraintName)
        {
            case EntitiesNames.Users.Constraints.PrimaryKey when context.ProcessedId is UserId userId:
                output = DomainErrorCatalog.Factories.CreateUserAlreadyExists(userId);
                return true;
            case EntitiesNames.RunningRecords.Constraints.PrimaryKey when context.ProcessedId is RunningRecordId runningRecordId:
                output = DomainErrorCatalog.Factories.CreateRunningRecordAlreadyExists(runningRecordId);
                return true;
            case EntitiesNames.Tags.Constraints.NameUnique when context.ProcessedId is TagId tagId:
                output = DomainErrorCatalog.Factories.CreateTagNameAlreadyExists(tagId);
                return true;
            default:
                output = null;
                return false;
        }
    }
}