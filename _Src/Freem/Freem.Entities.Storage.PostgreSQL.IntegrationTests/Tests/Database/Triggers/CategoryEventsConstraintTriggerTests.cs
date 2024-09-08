using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers;

public sealed class CategoryEventsConstraintTriggerTests : ConstraintTriggerTestsBase
{
    public CategoryEventsConstraintTriggerTests(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(EventAction.Created)]
    [InlineData(EventAction.Updated)]
    [InlineData(EventAction.Removed)]
    public async Task CategoryEvent_ShouldSuccess_WhenCategoryExists(EventAction action)
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;
        var category = factory.CreateCategory();

        await Context.Users.AddAsync(user);
        await Context.Categories.AddAsync(category);

        var @event = new CategoryEventEntity
        {
            Id = "id",
            CategoryId = category.Id,
            UserId = user.Id,
            Action = action,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await Context.Events.AddAsync(@event);

        await Context.ShouldNotThrowExceptionAsync();
    }
    
    [Fact]
    public async Task CategoryEvent_ShouldSuccess_WhenCategoryNotExistsAndActionIsRemoved()
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();

        var user = factory.User;

        await Context.Users.AddAsync(user);

        var @event = new CategoryEventEntity
        {
            Id = "id",
            CategoryId = "not_existed_id",
            UserId = user.Id,
            Action = EventAction.Removed,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await Context.Events.AddAsync(@event);

        await Context.ShouldNotThrowExceptionAsync();
    }
    
    [Theory]
    [InlineData(EventAction.Created)]
    [InlineData(EventAction.Updated)]
    public async Task CategoryEvent_ShouldThrowException_WhenCategoryDoesNotExist(EventAction action)
    {
        var factory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        
        var user = factory.User;

        await Context.Users.AddAsync(user);
        
        var @event = new CategoryEventEntity
        {
            Id = "id",
            CategoryId = "category_id",
            UserId = user.Id,
            Action = action,
            CreatedAt = DateTimeOffset.UtcNow
        };
        
        await Context.Events.AddAsync(@event);

        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.CategoriesEventsCategoryNotExist));
    }

    [Theory]
    [InlineData(EventAction.Created)]
    [InlineData(EventAction.Updated)]
    public async Task CategoryEvent_ShouldThrowException_WhenCategoryAndEventHaveDifferentUserIds(EventAction action)
    {
        var firstFactory = DatabaseEntitiesFactory.CreateFirstUserEntitiesFactory();
        var secondFactory = DatabaseEntitiesFactory.CreateSecondUserEntitiesFactory();
        
        var firstUser = firstFactory.User;
        var category = firstFactory.CreateCategory();

        await Context.Users.AddAsync(firstUser);
        await Context.Categories.AddAsync(category);
        
        var secondUser = secondFactory.User;
        
        await Context.Users.AddAsync(secondUser);

        var @event = new CategoryEventEntity
        {
            Id = "id",
            CategoryId = category.Id,
            UserId = secondUser.Id,
            Action = action,
            CreatedAt = DateTimeOffset.UtcNow
        };
        
        await Context.Events.AddAsync(@event);
        
        await Context.ShouldThrowExceptionAsync<PostgresException>(
            e => e.Message.Contains(TriggerErrorCodes.CategoriesEventsDifferentUserIds));
    }
}