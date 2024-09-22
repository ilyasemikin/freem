using Freem.Identifiers.Abstractions.Generators;
using Freem.Identifiers.Base;

namespace Freem.Identifiers.Implementations.Generators;

public sealed class GuidStringIdentifierGenerator<TIdentifier> : IIdentifierGenerator<TIdentifier>
    where TIdentifier : StringIdentifier
{
    public delegate TIdentifier Factory(string value);
    
    private readonly Factory _factory;

    public GuidStringIdentifierGenerator(Factory factory)
    {
        _factory = factory;
    }

    public TIdentifier Generate()
    {
        var value = Guid.NewGuid().ToString("N");
        return _factory(value);
    }
}