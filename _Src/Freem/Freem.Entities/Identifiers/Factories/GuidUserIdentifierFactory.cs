using Freem.Entities.Identifiers.Factories.Base;

namespace Freem.Entities.Identifiers.Factories;

public sealed class GuidUserIdentifierFactory : BaseGuidIdentifierEntityFactory<UserIdentifier>
{
    public GuidUserIdentifierFactory() 
        : base(value => new UserIdentifier(value))
    {
    }
}