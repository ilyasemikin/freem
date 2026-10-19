using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Records.Models;

public sealed class RecordName : IEquatable<RecordName>, IParsable<RecordName>
{
    private readonly string _value;

    public const int MaxLength = 128;
    
    private RecordName(string value)
    {
        _value = value;
    }

    public bool Equals(RecordName? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is RecordName other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(RecordName? left, RecordName? right)
    {
        if (left is null && right is null)
            return true;
        if (left is null || right is null)
            return false;
        
        return left.Equals(right);
    }

    public static bool operator !=(RecordName? left, RecordName? right)
    {
        return !(left == right);
    }

    public static RecordName Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for RecordName: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out RecordName? result)
    {
        if (s is null || s.Length > MaxLength)
        {
            result = null;
            return false;
        }

        result = new RecordName(s);
        return true;
    }
}