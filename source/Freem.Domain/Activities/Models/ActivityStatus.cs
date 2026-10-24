using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Activities.Models;

public sealed class ActivityStatus : IEquatable<ActivityStatus>, IParsable<ActivityStatus>
{
    private readonly Value _value;

    public static ActivityStatus Active { get; } = new(Value.Active);
    public static ActivityStatus Archived { get; } = new(Value.Archived);
    
    public ActivityStatus(Value value)
    {
        if (!Enum.IsDefined(value))
            throw new ArgumentOutOfRangeException(nameof(value), value, null);

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

    public Value ToValue()
    {
        return _value;
    }

    public static ActivityStatus Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for ActivityStatus: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out ActivityStatus? result)
    {
        if (!Enum.TryParse<Value>(s, out var value) || !Enum.IsDefined(value))
        {
            result = null;
            return false;
        }

        result = new ActivityStatus(value);
        return true;
    }
    
    public enum Value
    {
        Active,
        Archived
    }
}
