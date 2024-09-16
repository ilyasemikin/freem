using Freem.Entities.Identifiers.Factories.Base;

namespace Freem.Entities.Identifiers.Factories;

public sealed class GuidEventIdentifierFactory : BaseGuidIdentifierEntityFactory<EventIdentifier>
{
    public GuidEventIdentifierFactory() 
        : base(value => new EventIdentifier(value))
    {
    }
}