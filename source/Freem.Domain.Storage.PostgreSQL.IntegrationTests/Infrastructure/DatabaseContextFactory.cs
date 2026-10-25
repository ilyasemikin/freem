using Freem.Domain.Storage.PostgreSQL.DependencyInjection;
using Freem.Domain.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure;

internal static class DatabaseContextFactory
{
    public static DatabaseContextHandle Create(Action<string>? logger = null)
    {
        var configuration = ConfigurationProperties.Read();

        StorageOptions.LoggerAction? action = logger is not null
            ? message => logger(message)
            : null;
        var options = StorageOptionsFactory.Create(action);

        var services = new ServiceCollection();
        services.AddPostgreSqlStorage(configuration, options);

        var provider = services.BuildServiceProvider();
        return new DatabaseContextHandle(provider);
    }
}
