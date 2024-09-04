using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;

namespace Freem.Entities.Storage.PostgreSQL.Database.Errors.Abstractions;

public interface IDatabaseError : IEquatable<IDatabaseError>
{
    static bool TryParse(string input, [NotNullWhen(true)] out IDatabaseError? error)
    {
        if (DatabaseForeignKeyConstraintError.TryParse(input, out var foreignKeyConstraintError))
        {
            error = foreignKeyConstraintError;
            return true;
        }

        if (TriggerConstraintError.TryParse(input, out var triggerConstraintError))
        {
            error = triggerConstraintError;
            return true;
        }

        error = null;
        return false;
    }
}