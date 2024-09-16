namespace Freem.Entities.Storage.PostgreSQL.Database.Models;

internal sealed class DatabaseColumn : IEquatable<DatabaseColumn>
{
    public string Table { get; }
        
    public string Key { get; }
        
    public DatabaseColumn(string table, string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(table);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
            
        Table = table;
        Key = key;
    }

    public bool Equals(DatabaseColumn? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return Table == other.Table && Key == other.Key;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is DatabaseColumn other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Table, Key);
    }
}