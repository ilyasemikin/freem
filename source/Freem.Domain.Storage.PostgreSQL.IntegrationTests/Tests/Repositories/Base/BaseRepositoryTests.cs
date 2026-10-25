using Freem.Domain.Users;
using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.Database.Extensions;
using Freem.Domain.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Configuration;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;

[Collection("Sequential")]
public abstract class BaseRepositoryTests<TRepository> : IDisposable
    where TRepository : notnull
{
    private const string DefaultEntitiesUserId = "userId";
    
    private readonly ServiceProvider _provider;

    internal DatabaseContext Database { get; }
    internal TRepository Repository { get; }
    internal DatabaseEntitiesFactory EntitiesFactory { get; }

    internal BaseRepositoryTests(ITestOutputHelper output)
    {
        EntitiesFactory = new DatabaseEntitiesFactory(DefaultEntitiesUserId);
        
        var configuration = ConfigurationProperties.Read();
        var storageOptions = StorageOptionsFactory.Create(output.WriteLine);
        
        var services = new ServiceCollection();
        services.AddPostgreSqlStorage(configuration, storageOptions);

        var options = new ServiceProviderOptions { ValidateOnBuild = true };

        _provider = services.BuildServiceProvider(options);
        var provider = _provider;

        Database = provider.GetRequiredService<DatabaseContext>();
        Repository = provider.GetRequiredService<TRepository>();
        
        Database.TruncateTables();
    }
    
    internal UserId GenerateUserId()
    {
        return UserId.Generate();
    }
    
    public void Dispose()
    {
        Database.TruncateTables();
        
        Database.Dispose();
        _provider.Dispose();
        
        GC.SuppressFinalize(this);
    }
}