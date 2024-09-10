using Freem.Entities.Identifiers.Base;

namespace Freem.Entities.Identifiers;

public sealed class ActivityIdentifier : StringIdentifier, IEquatable<ActivityIdentifier>
{
    public ActivityIdentifier(string value) 
        : base(value)
    {
    }
    
    public bool Equals(ActivityIdentifier? other)
    {
        return other?.Value == Value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ActivityIdentifier other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(ActivityIdentifier left, ActivityIdentifier right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ActivityIdentifier left, ActivityIdentifier right)
    {
        return !(left == right);
    }
}
