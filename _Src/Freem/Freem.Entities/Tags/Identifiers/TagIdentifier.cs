using Freem.Entities.Abstractions.Identifiers;
using Freem.Identifiers.Base;

namespace Freem.Entities.Tags.Identifiers;

public sealed class TagIdentifier : StringIdentifier, IEntityIdentifier
{
    public TagIdentifier(string value) 
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

    public static implicit operator TagIdentifier(string value)
    {
        return new TagIdentifier(value);
    }
}
