using Freem.Identifiers.Base;

namespace Freem.Entities.Abstractions.Events.Identifiers;

public sealed class EventIdentifier : StringIdentifier
{
    public EventIdentifier(string value) : base(value)
    {
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is EventIdentifier other && base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static implicit operator EventIdentifier(string value)
    {
        return new EventIdentifier(value);
    }
}