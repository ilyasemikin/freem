using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
using Freem.Entities.UseCases.IntegrationTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures;

public sealed class DataManager
{
    private readonly TestsConfiguration _configuration;
    private readonly IServiceProvider _services;
    
    internal DataManager(TestsConfiguration configuration, IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(services);
        
        _configuration = configuration;
        _services = services;
    }

    public void Clean()
    {
        using var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;
        
        var context = provider.GetRequiredService<DatabaseContext>();
        context.TruncateTables();

        var connection = ConnectionMultiplexer.Connect(_configuration.RedisConnectionString);
        var servers = connection.GetServers();
        foreach (var server in servers)
            server.FlushDatabase();
    }
}