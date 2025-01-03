using Freem.Tokens.JWT.Implementations.AccessTokens;
using Freem.Tokens.JWT.Implementations.AccessTokens.Models;
using Freem.Tokens.JWT.Implementations.RefreshTokens;
using Freem.Tokens.JWT.Implementations.RefreshTokens.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Tokens.JWT.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccessTokens(this IServiceCollection services, AccessTokenSettings settings)
    {
        services.TryAddTransient(provider => ActivatorUtilities.CreateInstance<AccessTokenGenerator>(provider, settings));
        services.TryAddTransient(provider => ActivatorUtilities.CreateInstance<AccessTokenValidator>(provider, settings));

        return services;
    }

    public static IServiceCollection AddRefreshTokens(this IServiceCollection services, RefreshTokenSettings settings)
    {
        services.TryAddTransient(provider => ActivatorUtilities.CreateInstance<RefreshTokenGenerator>(provider, settings));
        services.TryAddTransient(provider => ActivatorUtilities.CreateInstance<RefreshTokenValidator>(provider, settings));
        
        return services;
    }
}