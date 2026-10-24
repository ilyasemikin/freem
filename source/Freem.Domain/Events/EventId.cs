using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Events;

public sealed class EventId : Id, IParsable<EventId>
{
    private EventId(string value)
        : base(value)
    {
    }

    public static EventId Generate()
    {
        var value = GenerateValue();
        return new EventId(value);
    }

    public static EventId Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var id)
            ? id
            : throw new FormatException($"Invalid value for EventId: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out EventId? result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = null;
            return false;
        }

        result = new EventId(s);
        return true;
    }
}
