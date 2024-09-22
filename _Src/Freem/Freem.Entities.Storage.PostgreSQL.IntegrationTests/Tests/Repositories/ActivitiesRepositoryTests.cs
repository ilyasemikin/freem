using Freem.EFCore.Extensions;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Activities;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;
using Freem.Entities.Users.Identifiers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories;

public sealed class ActivitiesRepositoryTests : BaseRepositoryTests<IActivitiesRepository>
{
    public ActivitiesRepositoryTests(
        ITestOutputHelper output) 
        : base(output)
    {
    }
    
    [Fact]
    public async Task CreateAsync_ShouldSuccess_WhenPassValid()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbTags = EntitiesFactory.CreateTags(2);

        await Database.AddRangeAsync(dbUser);
        await Database.AddRangeAsync(dbTags);
        await Database.SaveChangesAsync();

        var dbActivity = EntitiesFactory.CreateActivity();
        dbActivity.Tags = dbTags.ToList();
        
        // Act
        var activity = dbActivity.MapToDomainEntity();
        
        await Repository.CreateAsync(activity);

        // Assert
        var dbActivityActual = await Database.Activities.FindEntityAsync(activity.Id);

        Assert.Equal(dbActivity, dbActivityActual);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenRelatedTagNotExist()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbTag = EntitiesFactory.CreateTag();
        var dbActivity = EntitiesFactory.CreateActivity();
        dbActivity.Tags = [dbTag];

        await Database.Users.AddAsync(dbUser);
        await Database.SaveChangesAsync();

        // Act
        var activity = dbActivity.MapToDomainEntity();

        var exception = await Record.ExceptionAsync(() => Repository.CreateAsync(activity));
        
        // Assert
        Assert.NotNull(exception);
        
        var concreteException = Assert.IsType<NotFoundRelatedException>(exception);
        Assert.Equal(1, concreteException.RelatedIds.Count);
        Assert.Equal(activity.Tags.Identifiers.First(), concreteException.RelatedIds[0]);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldSuccess_WhenPassValid()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbTags = EntitiesFactory.CreateTags(2);
        
        var dbActivity = EntitiesFactory.CreateActivity();
        dbActivity.Tags = dbTags.ToList();

        await Database.AddRangeAsync(dbUser, dbActivity);
        await Database.AddRangeAsync(dbTags);
        await Database.SaveChangesAsync();
        Database.DetachEntry(dbActivity);
        
        // Act
        var dbUpdatedActivity = dbActivity.Clone();
        dbUpdatedActivity.Name = "Updated";
        dbUpdatedActivity.UpdatedAt = DateTimeOffset.UtcNow;

        var activity = dbUpdatedActivity.MapToDomainEntity();

        await Repository.UpdateAsync(activity);

        // Assert
        var dbActivityActual = await Database.Activities.FindEntityAsync(activity.Id);

        Assert.NotEqual(dbActivity, dbActivityActual);
        Assert.Equal(dbUpdatedActivity, dbActivityActual);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateCreatedEntity_WhenEntityIsNotUpdatedActually()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivity = EntitiesFactory.CreateActivity();
        
        await Database.AddRangeAsync(dbUser, dbActivity);
        await Database.SaveChangesAsync();

        var activity = dbActivity.MapToDomainEntity();
        
        // Act
        await Repository.UpdateAsync(activity);
        
        // Assert
        var dbActualActivity = await Database.Activities.FirstOrDefaultAsync(e => e.Id == activity.Id);

        Assert.NotNull(dbActualActivity);
        Assert.Null(dbActualActivity.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenEntityIsNotExists()
    {
        // Arrange
        var dbActivity = EntitiesFactory.CreateActivity();

        // Act
        var activity = dbActivity.MapToDomainEntity();
        
        var exception = await Record.ExceptionAsync(() => Repository.UpdateAsync(activity));
        
        // Assert
        Assert.NotNull(exception);
        
        var concreteException = Assert.IsType<NotFoundException>(exception);
        Assert.Equal(activity.Id, concreteException.Id);
    }

    [Fact]
    public async Task RemoveAsync_ShouldSuccess_WhenActivityExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbTags = EntitiesFactory.CreateTags(2);
        
        var dbActivity = EntitiesFactory.CreateActivity();
        dbActivity.Tags = dbTags.ToList();

        await Database.AddRangeAsync(dbUser, dbActivity);
        await Database.AddRangeAsync(dbTags);
        await Database.SaveChangesAsync();
        Database.DetachEntry(dbActivity);

        // Act
        var activity = dbActivity.MapToDomainEntity();
        
        await Repository.RemoveAsync(activity.Id);
        
        // Assert
        var dbActivityActual = await Database.Activities.FindEntityAsync(activity.Id);
        
        Assert.Null(dbActivityActual);
    }

    [Fact]
    public async Task RemoveAsync_ShouldThrowException_WhenEntityIsNotExists()
    {
        // Arrange
        var idValue = Guid.NewGuid().ToString();
        var id = new ActivityIdentifier(idValue);
        
        // Act
        var exception = await Record.ExceptionAsync(() => Repository.RemoveAsync(id));
        
        // Assert
        Assert.NotNull(exception);
        
        var concreteException = Assert.IsType<NotFoundException>(exception);
        Assert.Equal(id, concreteException.Id);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldSuccess_WhenEntityExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbTags = EntitiesFactory.CreateTags(2);
        var dbActivity = EntitiesFactory.CreateActivity();
        dbActivity.Tags = dbTags.ToList();
        
        await Database.AddRangeAsync(dbUser, dbActivity);
        await Database.AddRangeAsync(dbTags);
        await Database.SaveChangesAsync();

        // Act
        var id = new ActivityIdentifier(dbActivity.Id);
        
        var result = await Repository.FindByIdAsync(id);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Founded);
        Assert.NotNull(result.Entity);

        var activity = result.Entity;

        Assert.Equal(dbActivity.Id, activity.Id);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldFailure_WhenEntityNotExists()
    {
        // Act
        var idValue = Guid.NewGuid().ToString();
        var id = new ActivityIdentifier(idValue);

        var result = await Repository.FindByIdAsync(id);
        
        // Assert
        Assert.NotNull(result);
        Assert.False(result.Founded);
        Assert.Null(result.Entity);
    }

    [Fact]
    public async Task FindAsync_ShouldSuccess_WhenEntityExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivity = EntitiesFactory.CreateActivity();
        
        await Database.AddRangeAsync(dbUser, dbActivity);
        await Database.SaveChangesAsync();
        
        var activityId = new ActivityIdentifier(dbActivity.Id);
        var userId = new UserIdentifier(dbUser.Id);
        
        var ids = new ActivityAndUserIdentifiers(activityId, userId);
        
        // Act
        var result = await Repository.FindAsync(ids);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Founded);
        Assert.NotNull(result.Entity);
        
        var activity = result.Entity;
        
        Assert.Equal(dbActivity.Id, activity.Id);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public async Task FindAsync_ShouldFailure_WhenPartOfIdsNotExists(bool activityIdExists, bool userIdExists)
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivity = EntitiesFactory.CreateActivity();
        
        await Database.AddRangeAsync(dbUser, dbActivity);
        await Database.SaveChangesAsync();

        var activityIdValue = activityIdExists ? dbActivity.Id : Guid.NewGuid().ToString();
        var userIdValue = userIdExists ? dbUser.Id : Guid.NewGuid().ToString();
        
        var activityId = new ActivityIdentifier(activityIdValue);
        var userId = new UserIdentifier(userIdValue);
        
        var ids = new ActivityAndUserIdentifiers(activityId, userId);
        
        // Act
        var result = await Repository.FindAsync(ids);
        
        // Assert
        Assert.NotNull(result);
        Assert.False(result.Founded);
        Assert.Null(result.Entity);
    }
}