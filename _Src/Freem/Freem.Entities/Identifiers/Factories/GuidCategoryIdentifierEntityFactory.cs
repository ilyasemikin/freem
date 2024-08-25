using Freem.Entities.Identifiers.Factories.Base;

namespace Freem.Entities.Identifiers.Factories;

public sealed class GuidCategoryIdentifierEntityFactory : BaseGuidIdentifierEntityFactory<CategoryIdentifier>
{
    public GuidCategoryIdentifierEntityFactory() 
        : base(value => new CategoryIdentifier(value))
    {
    }
}