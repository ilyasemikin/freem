using Freem.Domain.Storage.PostgreSQL.DependencyInjection;
using Freem.Api.Public.Configuration.Instances;

namespace Freem.Api.Public.Configuration.Extensions;

internal static class ConfigurationManagerExtensions
{
    public static StorageConfiguration GetStorageConfiguration(this IConfiguration section)
    {
        var configuration = section.GetConfiguration();
        var postgres = configuration.Postgres;
        return new StorageConfiguration(postgres.ConnectionString)
        {
            SensitiveDataLogging = postgres.EnableSensitiveDataLogging
        };
    }
    
    public static string GetRedisConnectionString(this IConfiguration section)
    {
        var configuration = section.GetConfiguration();
        return configuration.Redis.ConnectionString;
    }
    
    public static OpenApiConfiguration GetOpenApiConfiguration(this IConfiguration section)
    {
        var configuration = section.GetConfiguration();
        return configuration.OpenApi;
    }

    public static ApiConfiguration GetApiConfiguration(this IConfiguration section)
    {
        var configuration = section.GetConfiguration();
        return configuration.Api;
    }

    public static EgoHubIntegrationConfiguration GetEgoHubIntegrationConfiguration(this IConfiguration section)
    {
        var configuration = section.GetConfiguration();
        return configuration.EgoHubIntegration;
    }

    public static CompositeConfiguration GetConfiguration(this IConfiguration section)
    {
        var configuration = section.Get<CompositeConfiguration>();
        if (configuration is null)
            throw new NullReferenceException("Configuration is null");
        
        return configuration;
    }
}