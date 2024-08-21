using Freem.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;

public abstract class BaseRepositoryTests<TRepository>
    where TRepository : notnull
{
    internal DatabaseContext Database { get; }
    internal TRepository Repository { get; }

    internal BaseRepositoryTests()
    {
        var configuration = TestsConfiguration
            .Read()
            .ToStorageConfiguration();
        
        var services = new ServiceCollection();
        services.AddPostgreSqlStorage(configuration);

        var provider = services.BuildAndValidateServiceProvider();

        Database = provider.GetRequiredService<DatabaseContext>();
        Repository = provider.GetRequiredService<TRepository>();
    }
}