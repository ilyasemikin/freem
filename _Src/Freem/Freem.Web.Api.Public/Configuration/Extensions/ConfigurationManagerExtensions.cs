using Freem.Crypto.Hashes.Abstractions.Models;
using Freem.Entities.Events.Producer.Kafka.Models;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection;
using Freem.Entities.Tokens.JWT.Implementations.AccessTokens.Models;
using Freem.Entities.Tokens.JWT.Implementations.RefreshTokens.Models;

namespace Freem.Web.Api.Public.Configuration.Extensions;

internal static class ConfigurationManagerExtensions
{
    public static HashAlgorithm GetPasswordCredentialsHashAlgorithm(this IConfiguration section)
    {
        var configuration = section.GetConfiguration();
        var name = configuration.PasswordCredentials.HashAlgorithmName;
        return new HashAlgorithm(name);
    }

    public static StorageConfiguration GetStorageConfiguration(this IConfiguration section)
    {
        var configuration = section.GetConfiguration();
        var postgres = configuration.Postgres;
        return new StorageConfiguration(postgres.ConnectionString)
        {
            SensitiveDataLogging = postgres.EnableSensitiveDataLogging
        };
    }
    
    public static Freem.Tokens.Blacklist.Redis.DependencyInjection.RedisConfiguration GetRedisTokensConfiguration(
        this IConfiguration section)
    {
        var configuration = section.GetConfiguration();
        return new Freem.Tokens.Blacklist.Redis.DependencyInjection.RedisConfiguration(
            configuration.Redis.ConnectionString,
            configuration.Redis.TokensBlackListKey);
    }
    
    public static Freem.Locking.Redis.DependencyInjection.Microsoft.RedisConfiguration GetRedisLockingConfiguration(
        this IConfiguration section)
    {
        var configuration = section.GetConfiguration();
        return new Freem.Locking.Redis.DependencyInjection.Microsoft.RedisConfiguration(
            configuration.Redis.ConnectionString);
    }
    
    public static KafkaProducerConfiguration GetKafkaProducerConfiguration(this IConfiguration section)
    {
        var configuration = section.GetConfiguration();
        return new KafkaProducerConfiguration(configuration.Kafka.BootstrapServers);
    }
    
    public static string GetTokensSecretKey(this IConfiguration section)
    {
        var configuration = section.GetConfiguration();
        return configuration.Tokens.SecretKey;
    }
    
    public static AccessTokenSettings GetAccessTokenSettings(this IConfiguration section)
    {
        var configuration = section.GetConfiguration();
        var tc = configuration.Tokens.Access;
        return new AccessTokenSettings(tc.Issuer, tc.Audience, tc.ExpirationPeriod);
    }
    
    public static RefreshTokenSettings GetRefreshTokenSettings(this IConfiguration section)
    {
        var configuration = section.GetConfiguration();
        var tc = configuration.Tokens.Refresh;
        return new RefreshTokenSettings(tc.Issuer, tc.Audience, tc.ExpirationPeriod);
    }

    public static CompositeConfiguration GetConfiguration(this IConfiguration section)
    {
        var configuration = section.Get<CompositeConfiguration>();
        if (configuration is null)
            throw new NullReferenceException("Configuration is null");
        
        return configuration;
    }
}