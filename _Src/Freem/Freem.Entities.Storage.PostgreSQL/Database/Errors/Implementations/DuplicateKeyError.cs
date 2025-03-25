using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Abstractions;
using Npgsql;

namespace Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;

internal class DuplicateKeyError : IDatabaseError
{
    private static readonly Regex MessageRegex = new(
        """
        duplicate key value violates unique constraint "(?<ConstraintName>[a-z_]+)"
        """,
        RegexOptions.Compiled);

    private const string ConstraintNameGroupName = "ConstraintName";
    
    public string ConstraintName { get; }

    public DuplicateKeyError(string constraintName)
    {
        ArgumentException.ThrowIfNullOrEmpty(constraintName);
        
        ConstraintName = constraintName;
    }
    
    public bool Equals(IDatabaseError? other)
    {
        return other is DatabaseForeignKeyConstraintError error && Equals(error);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is TriggerConstraintError other && Equals(other);
    }
    
    private bool Equals(DuplicateKeyError other)
    {
        return ConstraintName == other.ConstraintName;
    }

    public override int GetHashCode()
    {
        return ConstraintName.GetHashCode();
    }

    public static bool TryParse(PostgresException exception, [NotNullWhen(true)] out DuplicateKeyError? error)
    {
        error = null;

        var input = exception.MessageText;
        var match = MessageRegex.Match(input);
        if (!match.Groups.TryGetValue(ConstraintNameGroupName, out var constraintNameGroup))
            return false;
        
        error = new DuplicateKeyError(constraintNameGroup.Value);
        return true;
    }
}