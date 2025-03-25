namespace Freem.Entities.Activities.Models;

public sealed class ActivityName : IEquatable<ActivityName>
{
    private readonly string _value;

    public const int MaxLength = 128;
    
    public ActivityName(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        if (value.Length > MaxLength)
            throw new ArgumentException($"Length cannot be more than {MaxLength}", nameof(value));
        
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
    
    public static implicit operator ActivityName(string value)
    {
        return new ActivityName(value);
    }

    public static implicit operator string(ActivityName value)
    {
        return value._value;
    }
}