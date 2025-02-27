using Freem.Entities.Abstractions.Identifiers;
using Freem.Identifiers.Base;

namespace Freem.Entities.Abstractions.Events.Identifiers;

public sealed class EventIdentifier : StringIdentifier, IEntityIdentifier
{
    public EventIdentifier(string value) : base(value)
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

    public static implicit operator EventIdentifier(string value)
    {
        return new EventIdentifier(value);
    }
}