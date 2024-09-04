using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Identifiers.Base;

public abstract class StringIdentifier : IEntityIdentifier
{
    public string Value { get; }

    protected internal StringIdentifier(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Value = value;
    }

    public bool Equals(IEntityIdentifier? other)
    {
        if (other is null)
            return false;
        
        return other.GetType() == GetType() && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) 
            return false;
        if (ReferenceEquals(this, obj)) 
            return true;
        if (obj.GetType() != GetType()) 
            return false;
        return Equals((StringIdentifier)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static explicit operator string(StringIdentifier identifier)
    {
        return identifier.Value;
    }
}