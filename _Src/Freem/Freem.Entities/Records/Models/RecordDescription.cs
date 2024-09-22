namespace Freem.Entities.Records.Models;

public sealed class RecordDescription : IEquatable<RecordDescription>
{
    private readonly string _value;
    
    public const int MaxLength = 1024;

    public RecordDescription(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value.Length > MaxLength)
            throw new ArgumentException($"Length cannot be more than {MaxLength}", nameof(value));

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

    public static bool operator ==(RecordDescription left, RecordDescription right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(RecordDescription left, RecordDescription right)
    {
        return !(left == right);
    }

    public static implicit operator RecordDescription(string value)
    {
        return new RecordDescription(value);
    }

    public static implicit operator string(RecordDescription value)
    {
        return value._value;
    }
}