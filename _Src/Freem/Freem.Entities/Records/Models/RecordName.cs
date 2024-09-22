namespace Freem.Entities.Records.Models;

public sealed class RecordName : IEquatable<RecordName>
{
    private readonly string _value;

    public const int MaxLength = 128;
    
    public RecordName(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value.Length > MaxLength)
            throw new ArgumentException($"Length cannot be more than {MaxLength}", nameof(value));

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

    public static bool operator ==(RecordName left, RecordName right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(RecordName left, RecordName right)
    {
        return !(left == right);
    }

    public static implicit operator RecordName(string value)
    {
        return new RecordName(value);
    }

    public static implicit operator string(RecordName value)
    {
        return value._value;
    }
}