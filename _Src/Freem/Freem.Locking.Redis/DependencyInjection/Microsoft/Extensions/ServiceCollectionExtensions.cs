using Freem.Locking.Abstractions;
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
        services.TryAddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(configuration.ConnectionString));

        services.TryAddTransient<SimpleDistributedLocker>();
        services.TryAddTransient<IDistributedLocker>(provider => provider.GetRequiredService<SimpleDistributedLocker>());
        
        return services;
    }
}