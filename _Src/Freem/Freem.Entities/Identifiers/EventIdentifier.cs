using Freem.Entities.Identifiers.Base;

namespace Freem.Entities.Identifiers;

public sealed class EventIdentifier : StringIdentifier
{
    public EventIdentifier(string value) 
        : base(value)
    {
    }
}