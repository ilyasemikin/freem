using Freem.Entities.Tokens.JWT.Implementations.AccessTokens;
using Freem.Entities.Tokens.JWT.Implementations.AccessTokens.Models;
using Freem.Entities.Tokens.JWT.Implementations.RefreshTokens;
using Freem.Entities.Tokens.JWT.Implementations.RefreshTokens.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.Tokens.JWT.DependencyInjection;

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