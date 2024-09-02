namespace Freem.Entities.Storage.PostgreSQL.Database.Errors;

internal sealed class ErrorParameter : IEquatable<ErrorParameter>
{
    public string Name { get; }
    public string Value { get; }

    public ErrorParameter(string name, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Name = name;
        Value = value;
    }

    public bool Equals(ErrorParameter? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ErrorParameter other && Equals(other);
    }

    public override string ToString()
    {
        return $"{Name}={Value}";
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Value);
    }
}