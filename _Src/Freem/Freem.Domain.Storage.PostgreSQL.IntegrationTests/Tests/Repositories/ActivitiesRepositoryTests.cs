using Freem.Domain.Activities;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Domain.Users;
using Freem.Domain.Storage.PostgreSQL.Database.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Activities;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Repositories;

public sealed class ActivitiesRepositoryTests : BaseRepositoryTests<IActivityRepository>
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
        
        var writeResult = await Repository.CreateAsync(activity);

        // Assert
        Assert.True(writeResult.Success);

        var dbActivityActual = await Database.Activities.FindEntityAsync(activity.Id);

        Assert.Equal(dbActivity, dbActivityActual);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnError_WhenRelatedTagNotExist()
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

        var result = await Repository.CreateAsync(activity);
        
        // Assert
        Assert.False(result.Success);
        ErrorAssert.RelatedEntitiesNotFound(result.Error, activity.Tags.Ids.First());
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
        Database.Entry(dbActivity).State = EntityState.Detached;
        
        // Act
        var dbUpdatedActivity = dbActivity.Copy();
        dbUpdatedActivity.Name = "Updated";
        dbUpdatedActivity.UpdatedAt = DateTimeOffset.UtcNow;

        var activity = dbUpdatedActivity.MapToDomainEntity();

        var writeResult = await Repository.UpdateAsync(activity);

        // Assert
        Assert.True(writeResult.Success);

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
        var writeResult = await Repository.UpdateAsync(activity);
        
        // Assert
        Assert.True(writeResult.Success);

        var activityId = activity.Id.ToString();
        var dbActualActivity = await Database.Activities.FirstOrDefaultAsync(e => e.Id == activityId);

        Assert.NotNull(dbActualActivity);
        Assert.Null(dbActualActivity.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnError_WhenEntityIsNotExists()
    {
        // Arrange
        var dbActivity = EntitiesFactory.CreateActivity();

        // Act
        var activity = dbActivity.MapToDomainEntity();
        
        var result = await Repository.UpdateAsync(activity);
        
        // Assert
        Assert.False(result.Success);
        ErrorAssert.EntityNotFound(result.Error, activity.Id);
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
        Database.Entry(dbActivity).State = EntityState.Detached;

        // Act
        var activity = dbActivity.MapToDomainEntity();
        
        var writeResult = await Repository.DeleteAsync(activity.Id);
        
        // Assert
        Assert.True(writeResult.Success);

        var dbActivityActual = await Database.Activities.FindEntityAsync(activity.Id);
        
        Assert.Null(dbActivityActual);
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnError_WhenEntityIsNotExists()
    {
        // Arrange
        var id = ActivityId.Generate();
        
        // Act
        var result = await Repository.DeleteAsync(id);
        
        // Assert
        Assert.False(result.Success);
        ErrorAssert.EntityNotFound(result.Error, id);
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
        var id = ActivityId.Parse(dbActivity.Id, null);
        
        var result = await Repository.FindByIdAsync(id);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Value);

        var activity = result.Value;

        var actualId = activity.Id.ToString();
        Assert.Equal(dbActivity.Id, actualId);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldFailure_WhenEntityNotExists()
    {
        // Act
        var id = ActivityId.Generate();

        var result = await Repository.FindByIdAsync(id);
        
        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, result.Error.Type);
    }

    [Fact]
    public async Task FindAsync_ShouldSuccess_WhenEntityExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivity = EntitiesFactory.CreateActivity();
        
        await Database.AddRangeAsync(dbUser, dbActivity);
        await Database.SaveChangesAsync();
        
        var activityId = ActivityId.Parse(dbActivity.Id, null);
        var userId = UserId.Parse(dbUser.Id, null);
        
        var ids = new ActivityAndUserIds(activityId, userId);
        
        // Act
        var result = await Repository.FindByMultipleIdAsync(ids);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        
        var activity = result.Value;
        
        var actualId = activity.Id.ToString();
        Assert.Equal(dbActivity.Id, actualId);
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

        var activityId = activityIdExists
            ? ActivityId.Parse(dbActivity.Id, null)
            : ActivityId.Generate();
        var userId = userIdExists 
            ? UserId.Parse(dbUser.Id, null) 
            : GenerateUserId();
        
        var ids = new ActivityAndUserIds(activityId, userId);
        
        // Act
        var result = await Repository.FindByMultipleIdAsync(ids);
        
        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, result.Error.Type);
    }
}