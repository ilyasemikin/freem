using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.DataTransfer.Formats;

public sealed class FormatFileExtension : IEquatable<FormatFileExtension>, IParsable<FormatFileExtension>
{
    private readonly string _value;

    public const int MaxLength = 16;

    private FormatFileExtension(string value)
    {
        _value = value;
    }

    public bool Equals(FormatFileExtension? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is FormatFileExtension other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(FormatFileExtension left, FormatFileExtension right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FormatFileExtension left, FormatFileExtension right)
    {
        return !(left == right);
    }

    public static FormatFileExtension Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for FormatFileExtension: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out FormatFileExtension? result)
    {
        if (!IsValid(s))
        {
            result = null;
            return false;
        }

        result = new FormatFileExtension(s);
        return true;
    }

    private static bool IsValid([NotNullWhen(true)] string? s)
    {
        return !string.IsNullOrEmpty(s) && s.Length <= MaxLength && s.All(c => c is >= 'a' and <= 'z' or >= '0' and <= '9');
    }
}
