using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Events.Models;

public sealed class EventAction : IEquatable<EventAction>, IParsable<EventAction>
{
    private readonly string _value;

    private EventAction(string value)
    {
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

    public static EventAction Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for EventAction: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out EventAction? result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = null;
            return false;
        }

        result = new EventAction(s);
        return true;
    }
}