using Freem.Tokens.Abstractions;
using Freem.Tokens.Blacklist.Redis.Implementations;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Freem.Tokens.Blacklist.Redis.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedisTokesBlacklist(
        this IServiceCollection services, 
        RedisConfiguration configuration)
    {
        services.AddTransient<ITokensBlacklist>(_ => new TokensBlacklist(
            configuration.BlacklistKey,
            ConnectionMultiplexer.Connect(configuration.ConnectionString)));

        return services;
    }
}