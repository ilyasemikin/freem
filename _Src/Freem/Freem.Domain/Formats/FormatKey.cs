using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Formats;

public sealed class FormatKey : IEquatable<FormatKey>, IParsable<FormatKey>
{
    private readonly string _value;

    public const int MaxLength = 64;

    private FormatKey(string value)
    {
        _value = value;
    }

    public bool Equals(FormatKey? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is FormatKey other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(FormatKey left, FormatKey right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FormatKey left, FormatKey right)
    {
        return !(left == right);
    }

    public static FormatKey Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for FormatKey: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out FormatKey? result)
    {
        if (string.IsNullOrEmpty(s) || s.Length > MaxLength || !s.All(IsAllowed))
        {
            result = null;
            return false;
        }

        result = new FormatKey(s);
        return true;
    }

    private static bool IsAllowed(char c)
    {
        return c is >= 'a' and <= 'z' or >= '0' and <= '9' or '-';
    }
}
