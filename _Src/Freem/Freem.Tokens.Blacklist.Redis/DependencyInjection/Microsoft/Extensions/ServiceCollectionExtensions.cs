﻿using Freem.Tokens.Abstractions;
using Freem.Tokens.Blacklist.Redis.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace Freem.Tokens.Blacklist.Redis.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedisTokensBlacklist(
        this IServiceCollection services,
        Func<IServiceProvider, RedisConfiguration> configurationGetter)
    {
        services.TryAddSingleton<ITokensBlacklist>(provider =>
        {
            var configuration = configurationGetter(provider);
            return new TokensBlacklist(
                configuration.BlacklistKey,
                ConnectionMultiplexer.Connect(configuration.ConnectionString));
        });
        
        return services;
    }
    
    public static IServiceCollection AddRedisTokensBlacklist(
        this IServiceCollection services, 
        RedisConfiguration configuration)
    {
        return services.AddRedisTokensBlacklist(_ => configuration);
    }
}