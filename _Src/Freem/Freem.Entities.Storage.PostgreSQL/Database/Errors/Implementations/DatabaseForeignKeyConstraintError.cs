using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Abstractions;
using Freem.Entities.Storage.PostgreSQL.Database.Models;
using Npgsql;

namespace Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;

internal sealed class DatabaseForeignKeyConstraintError : IDatabaseError
{
    private static readonly Regex MessageRegex = new(
        @"insert or update on table ""(?<ConstaintTable>[a-z_]+)"" violates foreign key constraint ""(?<ConstaintName>[a-z_]+)",
        RegexOptions.Compiled);

    private static readonly Regex DetailRegex = new(
        @"DETAIL: Key \((?<Key>[A-z_]+)\)=\((?<Value>[A-z_0-9-]+)\) is not present in table ""(?<Table>[a-z_]+)""",
        RegexOptions.Compiled);

    private const string ConstraintTableGroupName = "ConstaintTable";
    private const string ConstraintNameGroupName = "ConstaintName";

    private const string TableGroupName = "Table";
    private const string KeyGroupName = "Key";
    private const string ValueGroupName = "Value";
    
    public ConstraintInfo Constraint { get; }
    public DatabaseColumnWithValue Column { get; }
    
    public DatabaseForeignKeyConstraintError(ConstraintInfo constraint, DatabaseColumnWithValue column)
    {
        ArgumentNullException.ThrowIfNull(constraint);
        ArgumentNullException.ThrowIfNull(column);
        
        Constraint = constraint;
        Column = column;
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
        return Constraint.Equals(other.Constraint) && Column.Equals(other.Column);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Constraint, Column);
    }
    
    public static bool TryParse(PostgresException exception, [NotNullWhen(true)] out DatabaseForeignKeyConstraintError? error)
    {
        error = null;
        
        var messageMatch = MessageRegex.Match(exception.MessageText);
        var detailMatch = DetailRegex.Match(exception.Message);
        if (!messageMatch.Success || !detailMatch.Success)
            return false;

        if (!TryExtractConstraint(messageMatch, out var constraint))
            return false;
        if (!TryExtractColumn(detailMatch, out var column))
            return false;
        
        error = new DatabaseForeignKeyConstraintError(constraint, column);
        return true;

        static bool TryExtractConstraint(Match match, [NotNullWhen(true)] out ConstraintInfo? constraint)
        {
            constraint = null;
            if (!match.Groups.TryGetValue(ConstraintTableGroupName, out var tableNameGroup) ||
                !match.Groups.TryGetValue(ConstraintNameGroupName, out var foreignKeyNameGroup))
                return false;

            var constantTable = tableNameGroup.Value;
            var constraintName = foreignKeyNameGroup.Value;
            
            constraint = new ConstraintInfo(constantTable, constraintName);
            return true;
        }

        static bool TryExtractColumn(Match match, [NotNullWhen(true)] out DatabaseColumnWithValue? columnWithValue)
        {
            columnWithValue = null;
            if (!match.Groups.TryGetValue(TableGroupName, out var tableGroup) ||
                !match.Groups.TryGetValue(KeyGroupName, out var keyGroup) ||
                !match.Groups.TryGetValue(ValueGroupName, out var valueGroup))
                return false;
        
            var table = tableGroup.Value;
            var key = keyGroup.Value;
            var value = valueGroup.Value;
            
            var column = new DatabaseColumn(table, key);
            
            columnWithValue = new DatabaseColumnWithValue(column, value);
            return true;
        }
    }

    public sealed class ConstraintInfo : IEquatable<ConstraintInfo>
    {
        public string Table { get; }
        public string Name { get; }

        public ConstraintInfo(string table, string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(table);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            
            Table = table;
            Name = name;
        }

        public bool Equals(ConstraintInfo? other)
        {
            if (other is null) 
                return false;
            if (ReferenceEquals(this, other)) 
                return true;
            return Table == other.Table && Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is ConstraintInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Table, Name);
        }
    }
}