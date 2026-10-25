using Freem.Domain.Storage.Repositories.Activities;
using Freem.Domain.Storage.Repositories.Events;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Storage.Repositories.RunningRecords;
using Freem.Domain.Storage.Repositories.Tags;
using Freem.Domain.Storage.Repositories.Users;
using Freem.Domain.Storage.PostgreSQL.Configuration;
using Freem.Domain.Storage.PostgreSQL.DependencyInjection;
using Freem.Domain.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Domain.Storage.PostgreSQL.UnitTests.Tests.DependencyInjection.Microsoft;

public sealed class ServiceCollectionExtensionsTests
{
    private static readonly PostgresConfiguration PostgresConfiguration =
        new("Host=127.0.0.1; Port=8080; UserID=user; Password=Password; Database=freem");

    private static readonly StorageOptions StorageOptions = new()
    {
        EnableServiceProviderCaching = false
    };
    
    [Fact]
    public void AddPostgreSqlStorage_ShouldBuildValidProvider()
    {
        var services = new ServiceCollection();

        services.AddPostgreSqlStorage(PostgresConfiguration, StorageOptions);

        var options = new ServiceProviderOptions { ValidateOnBuild = true };
        var exception = Record.Exception(() => services.BuildServiceProvider(options));
        
        Assert.Null(exception);
    }
    
    public static TheoryData<Type> EntityRepositoryTypes { get; } = new()
    {
        typeof(IActivityRepository),
        
        typeof(IRecordRepository),
        
        typeof(IRunningRecordRepository),
        
        typeof(ITagRepository),
        
        typeof(IUserRepository),
        
        typeof(IEventRepository),
    };

    [Theory]
    [MemberData(nameof(EntityRepositoryTypes))]
    public void AddPostgreSqlStorage_ShouldResolveRepository(Type type)
    {
        object Resolve()
        {
            var services = new ServiceCollection();
            services.AddPostgreSqlStorage(PostgresConfiguration, StorageOptions);

            var provider = services.BuildServiceProvider();
            return provider.GetRequiredService(type);
        }

        object? repository = null;
        var exception = Record.Exception(() => repository = Resolve());
        
        Assert.Null(exception);
        Assert.NotNull(repository);
        Assert.IsAssignableFrom(type, repository);
    }
}
