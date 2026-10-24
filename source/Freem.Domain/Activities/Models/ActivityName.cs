using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Activities.Models;

public sealed class ActivityName : IEquatable<ActivityName>, IParsable<ActivityName>
{
    private readonly string _value;

    public const int MaxLength = 128;
    
    private ActivityName(string value)
    {
        _value = value;
    }

    public bool Equals(ActivityName? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ActivityName other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(ActivityName left, ActivityName right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ActivityName left, ActivityName right)
    {
        return !(left == right);
    }

    public static ActivityName Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for ActivityName: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out ActivityName? result)
    {
        if (string.IsNullOrWhiteSpace(s) || s.Length > MaxLength)
        {
            result = null;
            return false;
        }

        result = new ActivityName(s);
        return true;
    }
}