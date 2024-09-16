using Freem.Entities.Identifiers.Factories.Base;

namespace Freem.Entities.Identifiers.Factories;

public sealed class GuidActivityIdentifierEntityFactory : BaseGuidIdentifierEntityFactory<ActivityIdentifier>
{
    public GuidActivityIdentifierEntityFactory() 
        : base(value => new ActivityIdentifier(value))
    {
    }
}