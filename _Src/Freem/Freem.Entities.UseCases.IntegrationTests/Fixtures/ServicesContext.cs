using Freem.Credentials.Password.Abstractions;
using Freem.Credentials.Password.Implementations;
using Freem.Crypto.Hashes.DependencyInjection.Microsoft.Extensions;
using Freem.DependencyInjection.Microsoft.Extensions;
using Freem.Entities._Common.DependencyInjection;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.UseCases.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.UseCases.Events.DependencyInjection.Microsoft;
using Freem.Entities.UseCases.Executors.DependencyInjection.Microsoft;
using Freem.Entities.UseCases.IntegrationTests.Infrastructure;
using Freem.Locking.Local.DependencyInjection;
using Freem.Time.DependencyInjection.Microsoft;
using Freem.Tokens.Blacklist.Redis.DependencyInjection;
using Freem.Tokens.Blacklist.Redis.DependencyInjection.Microsoft.Extensions;
using Freem.Tokens.JWT.DependencyInjection;
using Freem.Tokens.JWT.Implementations.AccessTokens.Models;
using Freem.Tokens.JWT.Implementations.RefreshTokens.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures;

public sealed class ServicesContext
{
    private readonly IServiceProvider _services;
    
    public RequestExecutor RequestExecutor { get; }
    public DataManager DataManager { get; }

    public ServicesContext()
    {
        _services = BuildServiceProvider();
        
        RequestExecutor = new RequestExecutor(_services);
        DataManager = new DataManager(_services);
    }
    
    private static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        var configuration = TestsConfiguration.Read();

        var storageConfiguration = new StorageConfiguration(configuration.PostgresConnectionString)
        {
            SensitiveDataLogging = true
        };

        var redisConfiguration = new RedisConfiguration(configuration.RedisConnectionString, "refresh_blacklist");
        
        services
            .AddUtcCurrentTimeGetter()
            .AddIdentifiersGenerators()
            .AddAccessTokens(new AccessTokenSettings("test", "test", TimeSpan.FromDays(1)))
            .AddRefreshTokens(new RefreshTokenSettings("test", "test", TimeSpan.FromDays(1)))
            .AddRedisTokensBlacklist(redisConfiguration)
            .AddEmptyLocking()
            .AddPostgreSqlStorage(storageConfiguration)
            .AddUseCases()
            .AddUseCaseExecutor();

        services.AddCryptoHashes();
        services.TryAddTransient<PasswordRawHasher>();
        services.TryAddTransient<ISaltGenerator, GuidSaltGenerator>();
        services.TryAddTransient<ICurrentPasswordHashAlgorithmGetter>(_ => new StaticCurrentPasswordHashAlgorithmGetter("SHA512"));
        
        services.AddEventConsumers();
        
        return services.BuildAndValidateServiceProvider();
    }
}