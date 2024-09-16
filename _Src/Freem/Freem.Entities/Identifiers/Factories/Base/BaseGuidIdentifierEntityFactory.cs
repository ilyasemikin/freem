using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Abstractions.Identifiers.Factories;

namespace Freem.Entities.Identifiers.Factories.Base;

public abstract class BaseGuidIdentifierEntityFactory<TEntityIdentifier> : IEntityIdentifierFactory<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    protected delegate TEntityIdentifier CreateIdentifier(string value);

    private readonly CreateIdentifier _factory;

    protected BaseGuidIdentifierEntityFactory(CreateIdentifier factory)
    {
        _factory = factory;
    }

    public TEntityIdentifier Create()
    {
        var guid = Guid.NewGuid();
        var value = guid.ToString("N");
        return _factory(value);
    }
}