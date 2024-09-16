using Freem.Entities.Identifiers.Base;

namespace Freem.Entities.Identifiers;

public sealed class EventIdentifier : StringIdentifier, IEquatable<EventIdentifier>
{
    public EventIdentifier(string value) 
        : base(value)
    {
    }
    
    public bool Equals(EventIdentifier? other)
    {
        return other?.Value == Value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is EventIdentifier other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(EventIdentifier left, EventIdentifier right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(EventIdentifier left, EventIdentifier right)
    {
        return !(left == right);
    }
}