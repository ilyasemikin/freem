using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Tags.Models;

public sealed class TagName : IEquatable<TagName>, IParsable<TagName>
{
    private readonly string _value;

    public const int MaxLength = 128;
    
    private TagName(string value)
    {
        _value = value;
    }

    public bool Equals(TagName? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is TagName other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(TagName left, TagName right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TagName left, TagName right)
    {
        return !(left == right);
    }

    public static TagName Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for TagName: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out TagName? result)
    {
        if (string.IsNullOrWhiteSpace(s) || s.Length > MaxLength)
        {
            result = null;
            return false;
        }

        result = new TagName(s);
        return true;
    }
}