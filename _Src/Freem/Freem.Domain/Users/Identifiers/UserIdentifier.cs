using Freem.Domain.Identifiers;
using Freem.Identifiers.Base;

namespace Freem.Domain.Users.Identifiers;

public sealed class UserIdentifier : StringIdentifier, IEntityIdentifier
{
    public UserIdentifier(string value)
        : base(value)
    {
    }

    public override bool Equals(object? other)
    {
        return base.Equals(other);
    }
    
    public bool Equals(IEntityIdentifier? other)
    {
        return base.Equals(other);
    }
    
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    
    public override string ToString()
    {
        return base.ToString();
    }

    public static implicit operator UserIdentifier(string value)
    {
        return new UserIdentifier(value);
    }
}