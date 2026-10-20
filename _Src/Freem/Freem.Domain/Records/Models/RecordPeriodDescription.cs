using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Records.Models;

public sealed class RecordPeriodDescription : IEquatable<RecordPeriodDescription>, IParsable<RecordPeriodDescription>
{
    private readonly string _value;
    
    public const int MaxLength = 1024;

    private RecordPeriodDescription(string value)
    {
        _value = value;
    }
    
    public bool Equals(RecordPeriodDescription? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is RecordPeriodDescription other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return _value;
    }

    public static bool operator ==(RecordPeriodDescription? left, RecordPeriodDescription? right)
    {
        if (left is null && right is null)
            return true;
        if (left is null || right is null)
            return false;
        
        return left.Equals(right);
    }

    public static bool operator !=(RecordPeriodDescription? left, RecordPeriodDescription? right)
    {
        return !(left == right);
    }

    public static RecordPeriodDescription Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for RecordPeriodDescription: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out RecordPeriodDescription? result)
    {
        if (s is null || s.Length > MaxLength)
        {
            result = null;
            return false;
        }

        result = new RecordPeriodDescription(s);
        return true;
    }
}