using System.Diagnostics.CodeAnalysis;
using Freem.Converters.Abstractions;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Converters;

internal sealed class DuplicateKeyErrorToExceptionConverter :
    IPossibleConverter<DatabaseContextWriteContext, DuplicateKeyError, Exception>
{
    private static readonly IReadOnlyDictionary<string, DuplicateKeyStorageException.ErrorCode> Map
        = new Dictionary<string, DuplicateKeyStorageException.ErrorCode>()
        {
            [EntitiesNames.UsersLoginCredentials.Constraints.LoginUnique] = DuplicateKeyStorageException.ErrorCode.DuplicateUserLogin,
            [EntitiesNames.RunningRecords.Constraints.PrimaryKey] = DuplicateKeyStorageException.ErrorCode.DuplicateRunningRecord,
            [EntitiesNames.Tags.Constraints.NameUnique] = DuplicateKeyStorageException.ErrorCode.DuplicateTagName
        };

    public bool TryConvert(
        DatabaseContextWriteContext context,
        DuplicateKeyError error,
        [NotNullWhen(true)] out Exception? output)
    {
        if (!Map.TryGetValue(error.ConstraintName, out var errorCode))
        {
            output = null;
            return false;
        }

        output = new DuplicateKeyStorageException(errorCode);
        return true;
    }
}