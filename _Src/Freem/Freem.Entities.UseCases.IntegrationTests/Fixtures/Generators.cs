using Freem.Entities.Activities.Identifiers;
using Freem.Identifiers.Abstractions;
using Freem.Identifiers.Abstractions.Generators;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures;

public class Generators
{
    private readonly IServiceProvider _services;

    public Generators(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        _services = services;
    }

    public ActivityIdentifier CreateActivityIdentifier()
    {
        return Create<ActivityIdentifier>();
    }

    private T Create<T>()
        where T : IIdentifier
    {
        using var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;

        var generator = provider.GetRequiredService<IIdentifierGenerator<T>>();
        return generator.Generate();
    }
}