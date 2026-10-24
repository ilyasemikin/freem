using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Formats;

public sealed class FormatName : IEquatable<FormatName>, IParsable<FormatName>
{
    private readonly string _value;

    public const int MaxLength = 128;

    private FormatName(string value)
    {
        _value = value;
    }

    public bool Equals(FormatName? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is FormatName other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(FormatName left, FormatName right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FormatName left, FormatName right)
    {
        return !(left == right);
    }

    public static FormatName Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for FormatName: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out FormatName? result)
    {
        if (!IsValid(s))
        {
            result = null;
            return false;
        }

        result = new FormatName(s);
        return true;
    }

    private static bool IsValid([NotNullWhen(true)] string? s)
    {
        return !string.IsNullOrWhiteSpace(s) && s.Length <= MaxLength;
    }
}
