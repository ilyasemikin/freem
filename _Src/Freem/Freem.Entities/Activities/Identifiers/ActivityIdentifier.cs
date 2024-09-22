using Freem.Entities.Abstractions.Identifiers;
using Freem.Identifiers.Base;

namespace Freem.Entities.Activities.Identifiers;

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
}
