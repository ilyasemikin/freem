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

    [Fact]
    public async Task Test()
    {
        var userId1 = "user1";
        var userId2 = "user2";
        var categoryId1 = "category1";
        var categoryId2 = "category2";
        var tagId1 = "tag1";
        var tagId2 = "tag2";

        await _database.Users.AddAsync(new UserEntity
        {
            Id = userId1,
            Nickname = "user_1"
        });
        await _database.Users.AddAsync(new UserEntity
        {
            Id = userId2,
            Nickname = "user_2"
        });

        await _database.Categories.AddAsync(new CategoryEntity
        {
            Id = categoryId1,
            Status = CategoryStatus.Active,
            UserId = userId1
        });
        await _database.Categories.AddAsync(new CategoryEntity
        {
            Id = categoryId2,
            Status = CategoryStatus.Active,
            UserId = userId2
        });

        await _database.Tags.AddAsync(new TagEntity
        {
            Id = tagId1,
            Name = "name",
            UserId = userId1
        });
        await _database.Tags.AddAsync(new TagEntity
        {
            Id = tagId2,
            Name = "name",
            UserId = userId2
        });

        await _database.SaveChangesAsync();
    }
}