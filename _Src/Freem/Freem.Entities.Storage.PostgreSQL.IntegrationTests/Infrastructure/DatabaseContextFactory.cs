using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure;

internal static class DatabaseContextFactory
{
    public static DatabaseContext Create(Action<string>? logger = null)
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
        return provider.GetRequiredService<DatabaseContext>();
    }
}
