namespace Freem.Domain.Events;

public sealed class EntityRef : IEquatable<EntityRef>
{
    public EntityType Type { get; }
    public string Value { get; }

    public EntityRef(EntityType type, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Type = type;
        Value = value;
    }

    public bool Equals(EntityRef? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Type == other.Type && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as EntityRef);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Value);
    }

    public override string ToString()
    {
        return Value;
    }
}
