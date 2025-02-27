using Freem.Enums.Exceptions;

namespace Freem.Entities.Activities.Models;

public sealed class ActivityStatus : IEquatable<ActivityStatus>
{
    private readonly Value _value;

    public static ActivityStatus Active { get; } = Value.Active;
    public static ActivityStatus Archived { get; } = Value.Archived;
    
    public ActivityStatus(Value value)
    {
        InvalidEnumValueException<Value>.ThrowIfValueInvalid(value);

        _value = value;
    }
    
    public bool Equals(ActivityStatus? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ActivityStatus other && Equals(other);
    }

    public override int GetHashCode()
    {
        return (int)_value;
    }

    public override string ToString()
    {
        return _value.ToString("G");
    }

    public static bool operator ==(ActivityStatus left, ActivityStatus right)
    {
        return left._value == right._value;
    }

    public static bool operator !=(ActivityStatus left, ActivityStatus right)
    {
        return !(left == right);
    }

    public static implicit operator ActivityStatus(Value value)
    {
        return new ActivityStatus(value);
    }

    public static implicit operator Value(ActivityStatus value)
    {
        return value._value;
    }
    
    public enum Value
    {
        Active,
        Archived
    }
}
