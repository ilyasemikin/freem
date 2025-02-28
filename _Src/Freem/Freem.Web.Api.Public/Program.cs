using System.Text.Json.Serialization;
using Freem.Credentials.Password.DependencyInjection.Microsoft.Extensions;
using Freem.Crypto.Hashes.Abstractions.Models;
using Freem.Crypto.Hashes.DependencyInjection.Microsoft.Extensions;
using Freem.Entities;
using Freem.Entities.DependencyInjection;
using Freem.Entities.Events;
using Freem.Entities.Events.Producer.Kafka.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Events.Producer.Kafka.Models;
using Freem.Entities.Serialization.Json;
using Freem.Entities.Serialization.Json.Activities.Identifiers;
using Freem.Entities.Serialization.Json.Activities.Models;
using Freem.Entities.Serialization.Json.Events;
using Freem.Entities.Serialization.Json.Events.Identifiers;
using Freem.Entities.Serialization.Json.Events.Models;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Tokens.JWT.DependencyInjection;
using Freem.Entities.Tokens.JWT.Implementations.AccessTokens.Models;
using Freem.Entities.Tokens.JWT.Implementations.RefreshTokens.Models;
using Freem.Entities.UseCases.DependencyInjection.Microsoft.Extensions;
using Freem.Locking.Redis.DependencyInjection.Microsoft.Extensions;
using Freem.Time.DependencyInjection.Microsoft;
using Freem.Tokens.Blacklist.Redis.DependencyInjection.Microsoft.Extensions;
using Freem.Tokens.DependencyInjection.Microsoft.Extensions;
using Freem.Web.Api.Public.Authentication.DependencyInjection.Microsoft;
using Freem.Web.Api.Public.Configuration.DependencyInjection.Microsoft.Extensions;
using Freem.Web.Api.Public.Services.DependencyInjection.Microsoft.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

builder.Services.AddConfiguration(builder.Configuration);

builder.Services
    .AddUtcCurrentTimeGetter()
    .AddHttpContextAccessor()
    .AddLogging();

builder.Services
    .AddCryptoHashes()
    .AddPasswordRawHahser()
    .AddGuidSaltGenerator()
    .AddStaticCurrentPasswordHashAlgorithmGetter(CreatePasswordCredentialHashAlgorithm);

builder.Services
    .AddPostgreSqlStorage(CreateStorageConfiguration)
    .AddSimpleRedisDistributedLocks(CreateRedisLockingConfiguration)
    .AddRedisTokensBlacklist(CreateRedisTokensConfiguration)
    .AddKafkaEventProduction(CreateKafkaProducerConfiguration);

builder.Services
    .AddUseCaseContextProvider()
    .AddIdentifiersGenerators()
    .AddEntitiesIdentifiersNameConverters()
    .AddEntitiesUseCases();

builder.Services
    .AddStaticSecurityKeyGetter(CreateTokensSecretKey)
    .AddAccessTokens(CreateAccessTokenSettings)
    .AddRefreshTokens(CreateRefreshTokenSettings);

builder.Services.AddAuthentication()
    .AddJwtBearerAuthentication();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        var converters = options.JsonSerializerOptions.Converters;
        EntitiesJsonSerialization.Populate(converters);
    });

var app = builder.Build();

app.MapControllers();

app.Run();

HashAlgorithm CreatePasswordCredentialHashAlgorithm(IServiceProvider provider)
{
    var configuration = provider.GetConfiguration();
    var name = configuration.PasswordCredentials.HashAlgorithmName;
    return new HashAlgorithm(name);
}

StorageConfiguration CreateStorageConfiguration(IServiceProvider provider)
{
    var configuration = provider.GetConfiguration();
    return new StorageConfiguration(configuration.Postgres.ConnectionString);
}

Freem.Tokens.Blacklist.Redis.DependencyInjection.RedisConfiguration CreateRedisTokensConfiguration(IServiceProvider provider)
{
    var configuration = provider.GetConfiguration();
    return new Freem.Tokens.Blacklist.Redis.DependencyInjection.RedisConfiguration(
        configuration.Redis.ConnectionString,
        configuration.Redis.TokensBlackListKey);
}

Freem.Locking.Redis.DependencyInjection.Microsoft.RedisConfiguration CreateRedisLockingConfiguration(IServiceProvider provider)
{
    var configuration = provider.GetConfiguration();
    return new Freem.Locking.Redis.DependencyInjection.Microsoft.RedisConfiguration(
        configuration.Redis.ConnectionString);
}

KafkaProducerConfiguration CreateKafkaProducerConfiguration(IServiceProvider provider)
{
    var configuration = provider.GetConfiguration();
    return new KafkaProducerConfiguration(configuration.Kafka.BootstrapServers);
}

string CreateTokensSecretKey(IServiceProvider provider)
{
    var configuration = provider.GetConfiguration();
    return configuration.Tokens.SecretKey;
}

AccessTokenSettings CreateAccessTokenSettings(IServiceProvider provider)
{
    var configuration = provider.GetConfiguration();
    var tc = configuration.Tokens.Access;
    return new AccessTokenSettings(tc.Issuer, tc.Audience, tc.ExpirationPeriod);
}

RefreshTokenSettings CreateRefreshTokenSettings(IServiceProvider provider)
{
    var configuration = provider.GetConfiguration();
    var tc = configuration.Tokens.Refresh;
    return new RefreshTokenSettings(tc.Issuer, tc.Audience, tc.ExpirationPeriod);
}