using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database;

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
}