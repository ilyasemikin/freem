using Freem.Credentials.Password.Abstractions;
using Freem.Credentials.Password.Implementations;
using Freem.Crypto.Hashes.Abstractions.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Credentials.Password.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPasswordRawHahser(this IServiceCollection services)
    {
        services.TryAddSingleton<PasswordRawHasher>();
        return services;
    }

    public static IServiceCollection AddGuidSaltGenerator(this IServiceCollection services)
    {
        services.TryAddSingleton<ISaltGenerator, GuidSaltGenerator>();
        return services;
    }

    public static IServiceCollection AddStaticCurrentPasswordHashAlgorithmGetter(
        this IServiceCollection services, Func<IServiceProvider, HashAlgorithm> algorithmGetter)
    {
        services.TryAddSingleton<ICurrentPasswordHashAlgorithmGetter>(provider =>
        {
            var algorithm = algorithmGetter(provider);
            return new StaticCurrentPasswordHashAlgorithmGetter(algorithm);
        });
        return services;
    }

    public static IServiceCollection AddStaticCurrentPasswordHashAlgorithmGetter(
        this IServiceCollection services, HashAlgorithm algorithm)
    {
        services.TryAddSingleton<ICurrentPasswordHashAlgorithmGetter>(_ => new StaticCurrentPasswordHashAlgorithmGetter(algorithm));
        
        return services;
    }
}