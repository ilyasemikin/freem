using Freem.Entities.Abstractions.Identifiers;
using Freem.Identifiers.Base;

namespace Freem.Entities.Identifiers;

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