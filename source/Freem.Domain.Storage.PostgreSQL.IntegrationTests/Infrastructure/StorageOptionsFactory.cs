using Freem.Domain.Storage.PostgreSQL.DependencyInjection;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure;

internal static class StorageOptionsFactory
{
    public static StorageOptions Create(StorageOptions.LoggerAction? logger = null)
    {
        return new StorageOptions
        {
            EnableServiceProviderCaching = false,
            Logger = logger
        };
    }
}
