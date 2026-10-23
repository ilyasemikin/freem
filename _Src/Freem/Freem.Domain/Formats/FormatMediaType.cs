using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Formats;

public sealed class FormatMediaType : IEquatable<FormatMediaType>, IParsable<FormatMediaType>
{
    private readonly string _value;

    public const int MaxLength = 127;

    private FormatMediaType(string value)
    {
        _value = value;
    }

    public bool Equals(FormatMediaType? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is FormatMediaType other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(FormatMediaType left, FormatMediaType right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FormatMediaType left, FormatMediaType right)
    {
        return !(left == right);
    }

    public static FormatMediaType Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for FormatMediaType: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out FormatMediaType? result)
    {
        if (!IsValid(s))
        {
            result = null;
            return false;
        }

        result = new FormatMediaType(s);
        return true;
    }

    private static bool IsValid([NotNullWhen(true)] string? s)
    {
        if (string.IsNullOrEmpty(s) || s.Length > MaxLength || s.Any(char.IsWhiteSpace))
            return false;

        var parts = s.Split('/');
        return parts.Length == 2 && parts.All(part => part.Length > 0);
    }
}
