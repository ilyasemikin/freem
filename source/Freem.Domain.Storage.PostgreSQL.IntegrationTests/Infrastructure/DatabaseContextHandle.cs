using Freem.Domain.Storage.PostgreSQL.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure;

internal sealed class DatabaseContextHandle : IDisposable
{
    private readonly ServiceProvider _provider;

    public DatabaseContext Context { get; }

    public DatabaseContextHandle(ServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        _provider = provider;
        Context = provider.GetRequiredService<DatabaseContext>();
    }

    public void Dispose()
    {
        Context.Dispose();
        _provider.Dispose();
    }
}
