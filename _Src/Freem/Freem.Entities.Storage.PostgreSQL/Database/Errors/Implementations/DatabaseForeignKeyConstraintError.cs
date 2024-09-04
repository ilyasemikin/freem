using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;

internal sealed class DatabaseForeignKeyConstraintError : IDatabaseError
{
    private static readonly Regex MessageRegex = new(
        @"insert or update on table ""(?<Table>[a-z_]+)"" violates foreign key constraint ""(?<ForeignKey>[a-z_]+)""");

    private const string TableNameGroupName = "Table";
    private const string ForeignKeyGroupName = "ForeignKey";
    
    public string TableName { get; }
    public string ForeignKeyName { get; }

    public DatabaseForeignKeyConstraintError(string tableName, string foreignKeyName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
        ArgumentException.ThrowIfNullOrWhiteSpace(foreignKeyName);
        
        TableName = tableName;
        ForeignKeyName = foreignKeyName;
    }
    
    public bool Equals(IDatabaseError? other)
    {
        return other is DatabaseForeignKeyConstraintError error && Equals(error);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is DatabaseForeignKeyConstraintError other && Equals(other);
    }
    
    private bool Equals(DatabaseForeignKeyConstraintError other)
    {
        return TableName == other.TableName && ForeignKeyName == other.ForeignKeyName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(TableName, ForeignKeyName);
    }
    
    public static bool TryParse(string input, [NotNullWhen(true)] out DatabaseForeignKeyConstraintError? error)
    {
        error = null;
        
        var match = MessageRegex.Match(input);
        if (!match.Success)
            return false;
        
        if (!match.Groups.TryGetValue(TableNameGroupName, out var tableNameGroup) ||
            !match.Groups.TryGetValue(ForeignKeyGroupName, out var foreignKeyNameGroup))
            return false;

        var tableName = tableNameGroup.Value;
        var foreignKeyName = foreignKeyNameGroup.Value;
        
        error = new DatabaseForeignKeyConstraintError(tableName, foreignKeyName);
        return true;
    }
}