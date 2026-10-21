using Freem.Domain.DependencyInjection;
using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.Database.Extensions;
using Freem.Domain.Storage.PostgreSQL.DependencyInjection;
using Freem.Domain.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.DependencyInjection.Microsoft.Extensions;
using Freem.Domain.UseCases.IntegrationTests.Infrastructure;
using Utilities.Locking.InMemory.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Utilities.Clock.Abstractions;
using Utilities.Clock.DependencyInjection.Microsoft;
using Utilities.UseCases.Abstractions;

namespace Freem.Domain.UseCases.IntegrationTests.Fixtures;

public sealed class TestContext
{
    private readonly TestsConfiguration _configuration;
    private readonly IServiceProvider _services;
    
    public TestContext()
    {
        _configuration = TestsConfiguration.Read();
        _services = BuildServiceProvider(_configuration);
    }

    public UseCaseExecutors CreateExecutor()
    {
        return UseCaseExecutors.Create(_services);
    }
    
    public DateTimeOffset GetCurrentDateTime()
    {
        using var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;
        var clock = provider.GetRequiredService<IClock>();
        return clock.Get();
    }

    public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(HandlerContext context, TRequest request)
        where TRequest : IRequest
    {
        using var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;
        
        var mediator = provider.GetRequiredService<IRequestMediator<HandlerContext>>();

        return await mediator.ExecuteAsync<TRequest, TResponse>(context, request);
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
            .AddPostgreSqlStorage(storageConfiguration)
            .AddEntitiesUseCases()
            .AddLogging(builder => builder.AddProvider(NullLoggerProvider.Instance));
        
        services.AddInMemoryLocking();

        var options = new ServiceProviderOptions { ValidateOnBuild = true };
        
        return services.BuildServiceProvider(options);
    }
}