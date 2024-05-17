using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Models;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests;

public sealed class MigrationsTests
{
    private readonly DatabaseContext _context;

    public MigrationsTests()
    {
        _context = DatabaseContextFactory.Create();
    }

    [Fact]
    public void Migrations_ShouldCreateNew_WhenModelsChangged()
    {
        var hasChanges = _context.Database.HasPendingModelChanges();

        Assert.False(hasChanges);
    }
}