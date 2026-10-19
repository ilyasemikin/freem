using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Records.Models;

public sealed class RecordDescription : IEquatable<RecordDescription>, IParsable<RecordDescription>
{
    private readonly string _value;
    
    public const int MaxLength = 1024;

    private RecordDescription(string value)
    {
        _value = value;
    }
    
    public bool Equals(RecordDescription? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is RecordDescription other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(RecordDescription? left, RecordDescription? right)
    {
        if (left is null && right is null)
            return true;
        if (left is null || right is null)
            return false;
        
        return left.Equals(right);
    }

    public static bool operator !=(RecordDescription? left, RecordDescription? right)
    {
        return !(left == right);
    }

    public static RecordDescription Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for RecordDescription: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out RecordDescription? result)
    {
        if (s is null || s.Length > MaxLength)
        {
            result = null;
            return false;
        }

        result = new RecordDescription(s);
        return true;
    }
}