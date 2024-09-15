using Freem.Entities.Storage.PostgreSQL.DependencyInjection;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Extensions;

internal static class TestsConfigurationExtensions
{
    public static StorageConfiguration ToStorageConfiguration(
        this TestsConfiguration configuration,
        StorageConfiguration.LoggerAction? logger = null)
    {
        return new StorageConfiguration(configuration.ConnectionString)
        {
            EnableServiceProviderCaching = false,
            SensitiveDataLogging = true,
            Logger = logger
        };
    }
}