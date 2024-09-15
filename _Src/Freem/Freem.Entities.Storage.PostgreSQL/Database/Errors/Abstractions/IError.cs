using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;
using Npgsql;

namespace Freem.Entities.Storage.PostgreSQL.Database.Errors.Abstractions;

public interface IDatabaseError : IEquatable<IDatabaseError>
{
    static bool TryParse(PostgresException exception, [NotNullWhen(true)] out IDatabaseError? error)
    {
        if (DatabaseForeignKeyConstraintError.TryParse(exception, out var foreignKeyConstraintError))
        {
            error = foreignKeyConstraintError;
            return true;
        }

        if (TriggerConstraintError.TryParse(exception, out var triggerConstraintError))
        {
            error = triggerConstraintError;
            return true;
        }

        error = null;
        return false;
    }
}