using Freem.DependencyInjection.Microsoft.Extensions;
using Freem.Identifiers.Abstractions.Generators;
using Freem.Identifiers.Implementations.Generators;
using Freem.Locking.Redis.Implementations.Simple;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace Freem.Locking.Redis.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSimpleRedisDistributedLocks(
        this IServiceCollection services, 
        RedisConfiguration configuration)
    {
        services.TryAddSingleton<IConnectionMultiplexer>(
            _ => ConnectionMultiplexer.Connect(configuration.ConnectionString));

        services.TryAddSingleton<IIdentifierGenerator<SimpleLockIdentifier>>(
            _ => new GuidStringIdentifierGenerator<SimpleLockIdentifier>(value => new SimpleLockIdentifier(value)));
        
        services.TryAddTransientServiceWithImplementedInterfaces<SimpleDistributedLocker>();
        
        return services;
    }
}