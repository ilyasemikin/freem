using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Database;

public sealed class MigrationsTests
{
    private readonly DatabaseContext _context;

    public MigrationsTests()
    {
        _context = DatabaseContextFactory.Create();
    }

    [Fact]
    public void Migrations_ShouldCreateNew_WhenModelsChanged()
    {
        var hasChanges = _context.Database.HasPendingModelChanges();

        Assert.False(hasChanges);
    }

    [Fact]
    public void Migrations_ShouldAllApplied()
    {
        var hasPending = _context.Database
            .GetPendingMigrations()
            .Any();

        Assert.False(hasPending);
    }
}