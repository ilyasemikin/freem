using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Events;

public sealed class EventType : IEquatable<EventType>, IParsable<EventType>
{
    private readonly string _value;

    private EventType(string value)
    {
        _value = value;
    }

    public bool Equals(EventType? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as EventType);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    public static EventType Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for EventType: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out EventType? result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = null;
            return false;
        }

        result = new EventType(s);
        return true;
    }
}
