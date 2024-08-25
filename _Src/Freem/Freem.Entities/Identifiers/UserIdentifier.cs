using Freem.Entities.Identifiers.Base;

namespace Freem.Entities.Identifiers;

public sealed class UserIdentifier : StringIdentifier, IEquatable<UserIdentifier>
{
    public UserIdentifier(string value)
        : base(value)
    {
    }

    public bool Equals(UserIdentifier? other)
    {
        return other?.Value == Value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is UserIdentifier other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(UserIdentifier left, UserIdentifier right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(UserIdentifier left, UserIdentifier right)
    {
        return !(left == right);
    }
}