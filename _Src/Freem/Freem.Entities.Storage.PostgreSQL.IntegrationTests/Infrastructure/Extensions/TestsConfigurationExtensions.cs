using Freem.Entities.Storage.PostgreSQL.DependencyInjection;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Extensions;

internal static class TestsConfigurationExtensions
{
    public static StorageConfiguration ToStorageConfiguration(this TestsConfiguration configuration)
    {
        return new StorageConfiguration(configuration.ConnectionString);
    }
}