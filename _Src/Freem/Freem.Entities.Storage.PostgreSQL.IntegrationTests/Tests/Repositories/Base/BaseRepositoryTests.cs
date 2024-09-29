using Freem.DependencyInjection.Microsoft.Extensions;
using Freem.Entities._Common.DependencyInjection;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Database.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Extensions;
using Freem.Identifiers.Abstractions;
using Freem.Identifiers.Abstractions.Generators;
using Freem.Identifiers.Implementations.Generators;
using Freem.Identifiers.Implementations.Generators.DependencyInjection.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;

[Collection("Sequential")]
public abstract class BaseRepositoryTests<TRepository> : IDisposable
    where TRepository : notnull
{
    private const string DefaultEntitiesUserId = "userId";
    
    internal DatabaseContext Database { get; }
    internal TRepository Repository { get; }
    internal DatabaseEntitiesFactory EntitiesFactory { get; }
    internal TypedIdentifierGenerator IdentifiersGenerator { get; }

    internal BaseRepositoryTests(ITestOutputHelper output)
    {
        EntitiesFactory = new DatabaseEntitiesFactory(DefaultEntitiesUserId);
        
        var configuration = TestsConfiguration
            .Read()
            .ToStorageConfiguration(output.WriteLine);
        
        var services = new ServiceCollection();
        services
            .AddIdentifiersGenerators()
            .AddPostgreSqlStorage(configuration);

        var provider = services.BuildAndValidateServiceProvider();

        Database = provider.GetRequiredService<DatabaseContext>();
        Repository = provider.GetRequiredService<TRepository>();

        var generators = provider.GetServices<IIdentifierGenerator<IIdentifier>>();
        IdentifiersGenerator = new TypedIdentifierGenerator(generators);
        
        Database.TruncateTables();
    }
    
    public void Dispose()
    {
        Database.TruncateTables();
        
        Database.Dispose();
        
        GC.SuppressFinalize(this);
    }
}