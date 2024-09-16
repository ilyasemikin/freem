using Freem.DependencyInjection.Microsoft;
using Freem.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.Storage.PostgreSQL.UnitTests.Tests.DependencyInjection.Microsoft;

public sealed class ServiceCollectionExtensionsTests
{
    private static readonly StorageConfiguration StorageConfiguration =
        new("Host=127.0.0.1; Port=8080; UserID=user; Password=Password; Database=freem");
    
    [Fact]
    public void AddPostgreSqlStorage_ShouldBuildValidProvider()
    {
        var services = new ServiceCollection();

        services.AddPostgreSqlStorage(StorageConfiguration);

        var exception = Record.Exception(() => services.BuildAndValidateServiceProvider());
        
        Assert.Null(exception);
    }
    
    public static TheoryData<Type> RepositoryTypes = new()
    {
        typeof(IActivitiesRepository),
        typeof(IRecordsRepository),
        typeof(IRunningRecordRepository),
        typeof(ITagsRepository),
        typeof(IUsersRepository),
        typeof(IEventsRepository)
    };

    [Theory]
    [MemberData(nameof(RepositoryTypes))]
    public void AddPostgreSqlStorage_ShouldResolveRepository(Type repositoryType)
    {
        object Resolve()
        {
            return Services.Resolve(services => services.AddPostgreSqlStorage(StorageConfiguration), repositoryType);
        }

        object? repository = null;
        var exception = Record.Exception(() => repository = Resolve());
        
        Assert.Null(exception);
        Assert.NotNull(repository);
        Assert.IsAssignableFrom(repositoryType, repository);
    }
    
    [Theory]
    [MemberData(nameof(RepositoryTypes))]
    public void AddUtcCurrentTimeGetter_ShouldAddGetterOnlyOneTime_WhenCallTwice(Type repositoryType)
    {
        var services = new ServiceCollection();

        services.AddPostgreSqlStorage(StorageConfiguration);
        services.AddPostgreSqlStorage(StorageConfiguration);

        var count = services.CountByServiceType(repositoryType);
        Assert.Equal(1, count);
    }
}