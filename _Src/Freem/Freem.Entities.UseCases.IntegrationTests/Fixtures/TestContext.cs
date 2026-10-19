using Freem.Domain.DependencyInjection;
using Freem.Entities.Events.Producer.DependencyInjection.Microsoft;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.UseCases.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.UseCases.IntegrationTests.Infrastructure;
using Freem.Entities.UseCases.IntegrationTests.Infrastructure.Locking;
using Freem.Locking.Abstractions;
using Freem.UseCases.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Utilities.Clock.Abstractions;
using Utilities.Clock.DependencyInjection.Microsoft;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures;

public sealed class TestContext
{
    private readonly TestsConfiguration _configuration;
    private readonly IServiceProvider _services;
    
    public TestContext()
    {
        _configuration = TestsConfiguration.Read();
        _services = BuildServiceProvider(_configuration);
    }

    public UseCasePlainExecutor CreateExecutor()
    {
        return UseCasePlainExecutor.Create(_services);
    }
    
    public DateTimeOffset GetCurrentDateTime()
    {
        using var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;
        var clock = provider.GetRequiredService<IClock>();
        return clock.Get();
    }

    public Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request)
        where TRequest : notnull
    {
        var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;
        
        var executor = provider.GetRequiredService<IUseCaseExecutor<UseCaseExecutionContext>>();

        return executor.ExecuteAsync<TRequest, TResponse>(UseCaseExecutionContext.Empty, request);
    }
    
    public Task<TResponse> ExecuteAsync<TRequest, TResponse>(UseCaseExecutionContext context, TRequest request)
        where TRequest : notnull
    {
        var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;
        
        var executor = provider.GetRequiredService<IUseCaseExecutor<UseCaseExecutionContext>>();

        return executor.ExecuteAsync<TRequest, TResponse>(context, request);
    }

    public void CleanDatabases()
    {
        using var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;
        
        var context = provider.GetRequiredService<DatabaseContext>();
        context.TruncateTables();

    }
    
    private static IServiceProvider BuildServiceProvider(TestsConfiguration configuration)
    {
        var services = new ServiceCollection();

        var storageConfiguration = new StorageConfiguration(configuration.PostgresConnectionString)
        {
            SensitiveDataLogging = true
        };

        services
            .AddUtcTruncatedToSecondsClock()
            .AddEntityIdNameConverters()
            .AddSingleton<IDistributedLocker, EmptyDistributedLocker>()
            .AddPostgreSqlStorage(storageConfiguration)
            .AddNullEventPublishing()
            .AddEntitiesUseCases()
            .AddLogging(builder => builder.AddProvider(NullLoggerProvider.Instance));
        
        var options = new ServiceProviderOptions { ValidateOnBuild = true };
        
        return services.BuildServiceProvider(options);
    }
}