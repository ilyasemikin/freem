using Freem.Credentials.Password.Abstractions;
using Freem.Credentials.Password.Implementations;
using Freem.Crypto.Hashes.DependencyInjection.Microsoft.Extensions;
using Freem.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Bus.Events.DependencyInjection.Microsoft;
using Freem.Entities.DependencyInjection;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Tokens.JWT.DependencyInjection;
using Freem.Entities.Tokens.JWT.Implementations.AccessTokens.Models;
using Freem.Entities.Tokens.JWT.Implementations.RefreshTokens.Models;
using Freem.Entities.UseCases.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.UseCases.IntegrationTests.Infrastructure;
using Freem.Identifiers.Abstractions;
using Freem.Identifiers.Abstractions.Generators;
using Freem.Locking.Local.DependencyInjection;
using Freem.Time.DependencyInjection.Microsoft;
using Freem.Tokens.Blacklist.Redis.DependencyInjection;
using Freem.Tokens.Blacklist.Redis.DependencyInjection.Microsoft.Extensions;
using Freem.Tokens.DependencyInjection.Microsoft.Extensions;
using Freem.UseCases.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures;

public sealed class TestContext
{
    private const string TokensBlacklistRedisKey = "refresh_blacklist";

    private const string TokensIssuer = "test";
    private const string TokensAudience = "test";
    private const string TokensSecurityKey = "0123456789_0123456789_0123456789";
    
    private static TimeSpan TokensExpiration { get; } = TimeSpan.FromDays(1);

    private readonly TestsConfiguration _configuration;
    private readonly IServiceProvider _services;
    
    public TestContext()
    {
        _configuration = TestsConfiguration.Read();
        _services = BuildServiceProvider(_configuration);
    }

    public UseCasePlainExecutor CreateExecutor()
    {
        return UseCasePlainExecutor.Create(_services);
    }
    
    public T CreateIdentifier<T>()
        where T : IIdentifier
    {
        using var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;

        var generator = provider.GetRequiredService<IIdentifierGenerator<T>>();
        return generator.Generate();
    }

    public Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request)
        where TRequest : notnull
    {
        var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;
        
        var executor = provider.GetRequiredService<IUseCaseExecutor<UseCaseExecutionContext>>();

        return executor.ExecuteAsync<TRequest, TResponse>(UseCaseExecutionContext.Empty, request);
    }
    
    public Task<TResponse> ExecuteAsync<TRequest, TResponse>(UseCaseExecutionContext context, TRequest request)
        where TRequest : notnull
    {
        var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;
        
        var executor = provider.GetRequiredService<IUseCaseExecutor<UseCaseExecutionContext>>();

        return executor.ExecuteAsync<TRequest, TResponse>(context, request);
    }

    public void CleanDatabases()
    {
        using var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;
        
        var context = provider.GetRequiredService<DatabaseContext>();
        context.TruncateTables();

        var connection = ConnectionMultiplexer.Connect(_configuration.RedisConnectionString);
        var servers = connection.GetServers();
        foreach (var server in servers)
            server.FlushDatabase();
    }
    
    private static IServiceProvider BuildServiceProvider(TestsConfiguration configuration)
    {
        var services = new ServiceCollection();

        var storageConfiguration = new StorageConfiguration(configuration.PostgresConnectionString)
        {
            SensitiveDataLogging = true
        };

        var redisConfiguration = new RedisConfiguration(configuration.RedisConnectionString, TokensBlacklistRedisKey);

        var accessTokensSettings = new AccessTokenSettings(TokensIssuer, TokensAudience, TokensExpiration);
        var refreshTokensSettings = new RefreshTokenSettings(TokensIssuer, TokensAudience, TokensExpiration);

        services
            .AddUtcCurrentTimeGetter()
            .AddIdentifiersGenerators()
            .AddStaticSecurityKeyGetter(TokensSecurityKey)
            .AddAccessTokens(accessTokensSettings)
            .AddRefreshTokens(refreshTokensSettings)
            .AddRedisTokensBlacklist(redisConfiguration)
            .AddEmptyLocking()
            .AddPostgreSqlStorage(storageConfiguration)
            .AddEntitiesUseCases();

        services.AddCryptoHashes();
        services.TryAddTransient<PasswordRawHasher>();
        services.TryAddTransient<ISaltGenerator, GuidSaltGenerator>();
        services.TryAddTransient<ICurrentPasswordHashAlgorithmGetter>(_ => new StaticCurrentPasswordHashAlgorithmGetter("SHA512"));
        
        services.AddEventConsumers();
        
        return services.BuildAndValidateServiceProvider();
    }
}