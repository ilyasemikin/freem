namespace Freem.Entities.Identifiers.Factories;

public sealed class GuidTagIdentifierFactory : BaseGuidIdentifierEntityFactory<TagIdentifier>
{
    public GuidTagIdentifierFactory() 
        : base(value => new TagIdentifier(value))
    {
    }
}