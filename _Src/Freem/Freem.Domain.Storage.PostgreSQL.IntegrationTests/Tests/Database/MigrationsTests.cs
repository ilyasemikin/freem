using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Database;

public sealed class MigrationsTests : IDisposable
{
    private readonly DatabaseContextHandle _handle;

    private DatabaseContext Context => _handle.Context;

    public MigrationsTests()
    {
        _handle = DatabaseContextFactory.Create();
    }

    public void Dispose()
    {
        _handle.Dispose();
    }

    [Fact]
    public void Migrations_ShouldCreateNew_WhenModelsChanged()
    {
        var hasChanges = Context.Database.HasPendingModelChanges();

        Assert.False(hasChanges);
    }

    [Fact]
    public void Migrations_ShouldAllApplied()
    {
        var hasPending = Context.Database
            .GetPendingMigrations()
            .Any();

        Assert.False(hasPending);
    }
}