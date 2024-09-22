using Freem.Identifiers.Abstractions.Generators;
using Freem.Identifiers.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Identifiers.Implementations.Generators.DependencyInjection;

public sealed class GuidStringIdentifierGeneratorsAdder
{
    private readonly IServiceCollection _services;
    private readonly HashSet<Type> _types;

    internal GuidStringIdentifierGeneratorsAdder(IServiceCollection services)
    {
        _services = services;
        _types = new HashSet<Type>();
    }
    
    public GuidStringIdentifierGeneratorsAdder Add<TIdentifier>(
        GuidStringIdentifierGenerator<TIdentifier>.Factory factory)
        where TIdentifier : StringIdentifier
    {
        var type = typeof(TIdentifier);
        if (!_types.Add(type))
            throw new InvalidOperationException($"\"{nameof(TIdentifier)}\" has already been added");
        
        var service = typeof(IIdentifierGenerator<TIdentifier>);
        var instance = new GuidStringIdentifierGenerator<TIdentifier>(factory);
        _services.AddSingleton(service, _ => instance);
        
        return this;
    }
}