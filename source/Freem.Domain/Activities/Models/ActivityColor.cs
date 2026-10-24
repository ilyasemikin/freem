using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Activities.Models;

public sealed class ActivityColor : IEquatable<ActivityColor>, IParsable<ActivityColor>
{
    private readonly string _value;

    public const int Length = 7;

    private ActivityColor(string value)
    {
        _value = value;
    }

    public bool Equals(ActivityColor? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ActivityColor other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(ActivityColor left, ActivityColor right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ActivityColor left, ActivityColor right)
    {
        return !(left == right);
    }

    public static ActivityColor Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for ActivityColor: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out ActivityColor? result)
    {
        if (!IsValid(s))
        {
            result = null;
            return false;
        }

        var normalized = s.ToLowerInvariant();
        result = new ActivityColor(normalized);
        return true;
    }

    private static bool IsValid([NotNullWhen(true)] string? s)
    {
        if (s is null || s.Length != Length || s[0] != '#')
            return false;

        for (var i = 1; i < s.Length; i++)
        {
            if (!char.IsAsciiHexDigit(s[i]))
                return false;
        }

        return true;
    }
}
