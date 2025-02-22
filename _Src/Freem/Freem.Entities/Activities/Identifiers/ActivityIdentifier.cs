using Freem.Entities.Abstractions.Identifiers;
using Freem.Identifiers.Base;

namespace Freem.Entities.Identifiers;

public sealed class ActivityIdentifier : StringIdentifier, IEntityIdentifier
{
    public ActivityIdentifier(string value) 
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

    public static implicit operator ActivityIdentifier(string value)
    {
        return new ActivityIdentifier(value);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
