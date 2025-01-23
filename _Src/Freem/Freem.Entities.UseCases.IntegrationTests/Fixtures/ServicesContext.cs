using Freem.Credentials.Password.Abstractions;
using Freem.Credentials.Password.Implementations;
using Freem.Crypto.Hashes.DependencyInjection.Microsoft.Extensions;
using Freem.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Bus.Events.DependencyInjection.Microsoft;
using Freem.Entities.DependencyInjection;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Tokens.JWT.DependencyInjection;
using Freem.Entities.Tokens.JWT.Implementations.AccessTokens.Models;
using Freem.Entities.Tokens.JWT.Implementations.RefreshTokens.Models;
using Freem.Entities.UseCases.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures.Samples;
using Freem.Entities.UseCases.IntegrationTests.Infrastructure;
using Freem.Locking.Local.DependencyInjection;
using Freem.Time.DependencyInjection.Microsoft;
using Freem.Tokens.Blacklist.Redis.DependencyInjection;
using Freem.Tokens.Blacklist.Redis.DependencyInjection.Microsoft.Extensions;
using Freem.Tokens.DependencyInjection.Microsoft.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures;

public sealed class ServicesContext
{
    private const string TokensBlacklistRedisKey = "refresh_blacklist";

    private const string TokensIssuer = "test";
    private const string TokensAudience = "test";
    private const string TokensSecurityKey = "0123456789_0123456789_0123456789";
    
    private static TimeSpan TokensExpiration { get; } = TimeSpan.FromDays(1);

    private readonly IServiceProvider _services;
    
    public RequestExecutor RequestExecutor { get; }
    public SamplesManager Samples { get; }
    public DataManager DataManager { get; }
    public Generators Generators { get; }

    public ServicesContext()
    {
        var configuration = TestsConfiguration.Read();
        var services = BuildServiceProvider(configuration);

        _services = services;
        
        RequestExecutor = new RequestExecutor(services);
        Samples = new SamplesManager(this);
        DataManager = new DataManager(configuration, services);
        Generators = new Generators(services);
    }

    public RequestPlainExecutor CreateExecutor()
    {
        return RequestPlainExecutor.Create(_services);
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