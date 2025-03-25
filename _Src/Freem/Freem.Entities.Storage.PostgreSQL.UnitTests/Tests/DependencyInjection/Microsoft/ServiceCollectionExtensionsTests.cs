using Freem.DependencyInjection.Microsoft;
using Freem.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.RunningRecords;
using Freem.Entities.RunningRecords.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Base.Write;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users;
using Freem.Entities.Users.Identifiers;
using Microsoft.Extensions.DependencyInjection;
using Activity = Freem.Entities.Activities.Activity;

namespace Freem.Entities.Storage.PostgreSQL.UnitTests.Tests.DependencyInjection.Microsoft;

public sealed class ServiceCollectionExtensionsTests
{
    private static readonly StorageConfiguration StorageConfiguration =
        new("Host=127.0.0.1; Port=8080; UserID=user; Password=Password; Database=freem")
        {
            EnableServiceProviderCaching = false
        };
    
    [Fact]
    public void AddPostgreSqlStorage_ShouldBuildValidProvider()
    {
        var services = new ServiceCollection();

        services.AddPostgreSqlStorage(StorageConfiguration);

        var exception = Record.Exception(() => services.BuildAndValidateServiceProvider());
        
        Assert.Null(exception);
    }
    
    public static TheoryData<Type> EntityRepositoryTypes { get; } = new()
    {
        typeof(IActivitiesRepository),
        typeof(IWriteRepository<Activity, ActivityIdentifier>),
        typeof(ISearchByIdRepository<Activity, ActivityIdentifier>),
        typeof(ISearchByMultipleIdsRepository<Activity, ActivityIdentifier, ActivityAndUserIdentifiers>),
        typeof(IMultipleSearchByFilterRepository<Activity, ActivityIdentifier, ActivitiesByUserFilter>),
        
        typeof(IRecordsRepository),
        typeof(IWriteRepository<Records.Record, RecordIdentifier>),
        typeof(ISearchByIdRepository<Records.Record, RecordIdentifier>),
        typeof(ISearchByMultipleIdsRepository<Records.Record, RecordIdentifier, RecordAndUserIdentifiers>),
        typeof(IMultipleSearchByFilterRepository<Records.Record, RecordIdentifier, RecordsByUserFilter>),
        
        typeof(IRunningRecordRepository),
        typeof(IWriteRepository<RunningRecord, RunningRecordIdentifier>),
        typeof(ISearchByIdRepository<RunningRecord, RunningRecordIdentifier>),
        
        typeof(ITagsRepository),
        typeof(IWriteRepository<Tag, TagIdentifier>),
        typeof(ISearchByIdRepository<Tag, TagIdentifier>),
        typeof(ISearchByMultipleIdsRepository<Tag, TagIdentifier, TagAndUserIdentifiers>),
        typeof(IMultipleSearchByFilterRepository<Tag, TagIdentifier, TagsByUserFilter>),
        
        typeof(IUsersRepository),
        typeof(IWriteRepository<User, UserIdentifier>),
        typeof(ISearchByIdRepository<User, UserIdentifier>),
        
        typeof(IEventsRepository),
        typeof(ICreateRepository<IEntityEvent<IEntityIdentifier, UserIdentifier>, EventIdentifier>),
        typeof(ISearchByIdRepository<IEntityEvent<IEntityIdentifier, UserIdentifier>, EventIdentifier>),
        typeof(IMultipleSearchByFilterRepository<IEntityEvent<IEntityIdentifier, UserIdentifier>, EventIdentifier, EventsAfterTimeFilter>),
    };

    [Theory]
    [MemberData(nameof(EntityRepositoryTypes))]
    public void AddPostgreSqlStorage_ShouldResolveRepository(Type type)
    {
        object Resolve()
        {
            return Services.Resolve(services => services.AddPostgreSqlStorage(StorageConfiguration), type);
        }

        object? repository = null;
        var exception = Record.Exception(() => repository = Resolve());
        
        Assert.Null(exception);
        Assert.NotNull(repository);
        Assert.IsAssignableFrom(type, repository);
    }
    
    [Theory]
    [MemberData(nameof(EntityRepositoryTypes))]
    public void AddUtcCurrentTimeGetter_ShouldAddGetterOnlyOneTime_WhenCallTwice(Type repositoryType)
    {
        var services = new ServiceCollection();

        services.AddPostgreSqlStorage(StorageConfiguration);
        services.AddPostgreSqlStorage(StorageConfiguration);

        var count = services.Count(service => service.ServiceType == repositoryType);
        Assert.Equal(1, count);
    }
}