using Freem.Domain.Storage.PostgreSQL.DependencyInjection;
using Freem.Domain.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure;

internal static class DatabaseContextFactory
{
    public static DatabaseContextHandle Create(Action<string>? logger = null)
    {
        var configuration = TestsConfiguration.Read();
        var connectionString = configuration.ConnectionString;

        var storageConfiguration = new StorageConfiguration(connectionString)
        {
            EnableServiceProviderCaching = false,
            SensitiveDataLogging = true,
            Logger = logger is not null
                ? message => logger(message)
                : null
        };

        var services = new ServiceCollection();
        services.AddPostgreSqlStorage(storageConfiguration);

        var provider = services.BuildServiceProvider();
        return new DatabaseContextHandle(provider);
    }
}
