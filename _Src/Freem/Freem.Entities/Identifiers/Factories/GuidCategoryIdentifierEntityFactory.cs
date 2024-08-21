namespace Freem.Entities.Identifiers.Factories;

public sealed class GuidCategoryIdentifierEntityFactory : BaseGuidIdentifierEntityFactory<CategoryIdentifier>
{
    public GuidCategoryIdentifierEntityFactory() 
        : base(value => new CategoryIdentifier(value))
    {
    }
}