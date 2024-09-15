using Freem.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Database.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;

[Collection("Sequential")]
public abstract class BaseRepositoryTests<TRepository> : IDisposable
    where TRepository : notnull
{
    internal DatabaseContext Database { get; }
    internal TRepository Repository { get; }
    internal DatabaseEntitiesFactory EntitiesFactory { get; }

    internal BaseRepositoryTests(ITestOutputHelper output)
    {
        EntitiesFactory = new DatabaseEntitiesFactory("userId");
        
        var configuration = TestsConfiguration
            .Read()
            .ToStorageConfiguration(output.WriteLine);
        
        var services = new ServiceCollection();
        services.AddPostgreSqlStorage(configuration);

        var provider = services.BuildAndValidateServiceProvider();

        Database = provider.GetRequiredService<DatabaseContext>();
        Repository = provider.GetRequiredService<TRepository>();
        
        Database.TruncateTables();
    }

    public void Dispose()
    {
        Database.TruncateTables();
        
        Database.Dispose();
        
        GC.SuppressFinalize(this);
    }
}