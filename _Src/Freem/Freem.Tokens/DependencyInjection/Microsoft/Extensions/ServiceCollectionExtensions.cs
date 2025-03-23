using Freem.Tokens.Abstractions;
using Freem.Tokens.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Tokens.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStaticSecurityKeyGetter(this IServiceCollection services, string value)
    {
        services.TryAddSingleton<ISecurityKeyGetter>(_ => new StaticSecurityKeyGetter(value));
        
        return services;
    }
}