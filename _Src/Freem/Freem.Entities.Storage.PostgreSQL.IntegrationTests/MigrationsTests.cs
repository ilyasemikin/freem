using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Models;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests;

public class MigrationsTests
{
    private readonly DatabaseContext _database;

    public MigrationsTests()
    {
        _database = InfrastructureConfiguration.CreateDatabaseContext();
    }

    [Fact]
    public void Migrations_ShouldCreateNew_WhenModelsChangged()
    {
        var hasChanges = _database.Database.HasPendingModelChanges();

        Assert.False(hasChanges);
    }
}