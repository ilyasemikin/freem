using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Activities;

public sealed class ActivityId : Id, IParsable<ActivityId>
{
    private ActivityId(string value)
        : base(value)
    {
    }

    public static ActivityId Generate()
    {
        var value = GenerateValue();
        return new ActivityId(value);
    }

    public static ActivityId Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var id)
            ? id
            : throw new FormatException($"Invalid value for ActivityId: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out ActivityId? result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = null;
            return false;
        }

        result = new ActivityId(s);
        return true;
    }
}
