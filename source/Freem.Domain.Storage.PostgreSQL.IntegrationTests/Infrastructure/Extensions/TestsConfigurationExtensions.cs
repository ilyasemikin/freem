using Freem.Domain.Storage.PostgreSQL.DependencyInjection;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure.Extensions;

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