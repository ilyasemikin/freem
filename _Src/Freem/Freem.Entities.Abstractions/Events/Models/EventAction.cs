namespace Freem.Entities.Abstractions.Events.Models;

public sealed class EventAction : IEquatable<EventAction>
{
    private readonly string _value;

    public EventAction(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        _value = value;
    }
    
    public bool Equals(EventAction? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is EventAction other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(EventAction left, EventAction right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(EventAction left, EventAction right)
    {
        return !(left == right);
    }

    public static implicit operator string(EventAction value)
    {
        return value._value;
    }

    public static implicit operator EventAction(string value)
    {
        return new EventAction(value);
    }
}