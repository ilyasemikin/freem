namespace Freem.Entities.Identifiers.Factories;

public class GuidEventIdentifierFactory : BaseGuidIdentifierEntityFactory<EventIdentifier>
{
    public GuidEventIdentifierFactory() 
        : base(value => new EventIdentifier(value))
    {
    }
}