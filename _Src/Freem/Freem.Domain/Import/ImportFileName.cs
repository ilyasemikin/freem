using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Import;

public sealed class ImportFileName : IEquatable<ImportFileName>, IParsable<ImportFileName>
{
    private readonly string _value;

    public const int MaxLength = 255;

    private ImportFileName(string value)
    {
        _value = value;
    }

    public bool Equals(ImportFileName? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ImportFileName other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(ImportFileName left, ImportFileName right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ImportFileName left, ImportFileName right)
    {
        return !(left == right);
    }

    public static ImportFileName Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for ImportFileName: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out ImportFileName? result)
    {
        if (!IsValid(s))
        {
            result = null;
            return false;
        }

        result = new ImportFileName(s);
        return true;
    }

    private static bool IsValid([NotNullWhen(true)] string? s)
    {
        return !string.IsNullOrWhiteSpace(s) &&
               s.Length <= MaxLength &&
               s.IndexOfAny(['/', '\\']) < 0;
    }
}
