using Freem.UseCases.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.UseCases.Implementations.Microsoft;

public sealed class MicrosoftAdapterUseCaseResolver : IUseCaseResolver
{
    private readonly IServiceProvider _services;

    public MicrosoftAdapterUseCaseResolver(IServiceProvider services)
    {
        _services = services;
    }

    public object Resolve(Type type)
    {
        return _services.GetRequiredService(type);
    }
}