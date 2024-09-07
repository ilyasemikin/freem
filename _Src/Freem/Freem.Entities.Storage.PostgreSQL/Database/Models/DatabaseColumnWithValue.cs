namespace Freem.Entities.Storage.PostgreSQL.Database.Models;

internal sealed class DatabaseColumnWithValue : IEquatable<DatabaseColumnWithValue>
{
    public DatabaseColumn Column { get; }
    public string Value { get; }
    
    public DatabaseColumnWithValue(DatabaseColumn column, string value)
    {
        ArgumentNullException.ThrowIfNull(column);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        
        Column = column;
        Value = value;
    }

    public bool Equals(DatabaseColumnWithValue? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return Column.Equals(other.Column) && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is DatabaseColumnWithValue other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Column, Value);
    }
}