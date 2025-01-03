using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures;

public sealed class DataManager
{
    private readonly IServiceProvider _services;
    
    internal DataManager(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        _services = services;
    }

    public void Clean()
    {
        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        context.TruncateTables();
    }
}