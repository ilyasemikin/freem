using Freem.Domain;
using Freem.Domain.RunningRecords;
using Freem.Domain.Storage.Repositories.RunningRecords;
using Freem.Domain.Users;
using Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Repositories;

public sealed class RunningRecordsRepositoryTests : BaseRepositoryTests<IRunningRecordRepository>
{
    public RunningRecordsRepositoryTests(
        ITestOutputHelper output) 
        : base(output)
    {
    }

    [Fact]
    public async Task CreateAsync_ShouldSuccess_WhenPassValid()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbTags = EntitiesFactory.CreateTags(2);
        
        await Database.AddRangeAsync(dbUser);
        await Database.AddRangeAsync(dbActivities);
        await Database.AddRangeAsync(dbTags);
        await Database.SaveChangesAsync();
        
        var dbRecord = EntitiesFactory.CreateRunningRecord();
        dbRecord.Activities = dbActivities.ToList();
        dbRecord.Tags = dbTags.ToList();

        // Act
        var record = dbRecord.MapToDomainEntity();
        
        var writeResult = await Repository.CreateAsync(record);

        // Assert
        Assert.True(writeResult.Success);

        var recordId = record.Id.ToString();
        var dbRecordActual = await Database.RunningRecords.FirstOrDefaultAsync(e => e.UserId == recordId);

        Assert.Equal(dbRecord, dbRecordActual);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnError_WhenRelatedActivityNotExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivity = EntitiesFactory.CreateActivity();
        var dbRecord = EntitiesFactory.CreateRunningRecord();
        dbRecord.Activities = [dbActivity];
        
        await Database.Users.AddAsync(dbUser);
        await Database.SaveChangesAsync();

        // Act
        var record = dbRecord.MapToDomainEntity();
        
        var result = await Repository.CreateAsync(record);
        
        // Assert
        Assert.False(result.Success);
        ErrorAssert.RelatedEntitiesNotFound(result.Error, record.Activities.Ids.First());
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnError_WhenRelatedTagNotExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivity = EntitiesFactory.CreateActivity();
        var dbTag = EntitiesFactory.CreateTag();
        var dbRecord = EntitiesFactory.CreateRunningRecord();
        dbRecord.Activities = [dbActivity];
        dbRecord.Tags = [dbTag];
        
        await Database.AddRangeAsync(dbUser, dbActivity);
        await Database.SaveChangesAsync();

        // Act
        var record = dbRecord.MapToDomainEntity();
            
        var result = await Repository.CreateAsync(record);
        
        // Assert
        Assert.False(result.Success);
        ErrorAssert.RelatedEntitiesNotFound(result.Error, record.Tags.Ids.First());
    }

    [Fact]
    public async Task UpdateAsync_ShouldSuccess_WhenPassValid()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbTags = EntitiesFactory.CreateTags(2);
        
        var dbRecord = EntitiesFactory.CreateRunningRecord();
        dbRecord.Activities = dbActivities.ToList();
        dbRecord.Tags = dbTags.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.AddRangeAsync(dbActivities);
        await Database.AddRangeAsync(dbTags);
        await Database.SaveChangesAsync();
        Database.Entry(dbRecord).State = EntityState.Detached;

        // Act
        var dbUpdatedRecord = dbRecord.Copy();
        dbUpdatedRecord.Name = "Updated";
        dbUpdatedRecord.Description = "Updated";
        dbUpdatedRecord.UpdatedAt = DateTimeOffset.UtcNow;

        var record = dbUpdatedRecord.MapToDomainEntity();
        
        var writeResult = await Repository.UpdateAsync(record);
        
        // Assert
        Assert.True(writeResult.Success);

        var userId = record.UserId.ToString();
        var dbRecordActual = await Database.RunningRecords
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.UserId == userId);
        
        Assert.NotEqual(dbRecord, dbRecordActual);
        Assert.Equal(dbUpdatedRecord, dbRecordActual);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateCreatedEntity_WhenEntityIsNotUpdatedActually()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbRecord = EntitiesFactory.CreateRunningRecord();
        dbRecord.Activities = dbActivities.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.AddRangeAsync(dbActivities);
        await Database.SaveChangesAsync();

        var record = dbRecord.MapToDomainEntity();
        
        // Act
        var writeResult = await Repository.UpdateAsync(record);
        
        // Assert
        Assert.True(writeResult.Success);

        var dbActualRecord = await Database.RunningRecords
            .Include(e => e.Activities)
            .FirstOrDefaultAsync(e => e.UserId == dbUser.Id);
        
        Assert.NotNull(dbActualRecord);
        Assert.Null(dbActualRecord.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnError_WhenEntityNotExists()
    {
        // Arrange
        var dbRecord = EntitiesFactory.CreateRunningRecord();
        dbRecord.Activities = [EntitiesFactory.CreateActivity()];

        // Act
        var record = dbRecord.MapToDomainEntity();
        
        var result = await Repository.UpdateAsync(record);
        
        // Assert
        Assert.False(result.Success);
        ErrorAssert.EntityNotFound(result.Error, record.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnError_WhenRelatedActivityNotExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbTags = EntitiesFactory.CreateTags(2);
        
        var dbRecord = EntitiesFactory.CreateRunningRecord();
        dbRecord.Activities = dbActivities.ToList();
        dbRecord.Tags = dbTags.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.AddRangeAsync(dbActivities);
        await Database.AddRangeAsync(dbTags);
        await Database.SaveChangesAsync();
        Database.Entry(dbRecord).State = EntityState.Detached;
        
        // Act
        dbRecord.Activities.Add(EntitiesFactory.CreateActivity());

        var record = dbRecord.MapToDomainEntity();
        
        var result = await Repository.UpdateAsync(record);
        
        // Assert
        Assert.False(result.Success);
        ErrorAssert.RelatedEntitiesNotFound(result.Error, record.Activities.Ids.Last());
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnError_WhenRelatedTagNotExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbTags = EntitiesFactory.CreateTags(2);
        
        var dbRecord = EntitiesFactory.CreateRunningRecord();
        dbRecord.Activities = dbActivities.ToList();
        dbRecord.Tags = dbTags.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.AddRangeAsync(dbActivities);
        await Database.AddRangeAsync(dbTags);
        await Database.SaveChangesAsync();
        Database.Entry(dbRecord).State = EntityState.Detached;
        
        // Act
        dbRecord.Tags.Add(EntitiesFactory.CreateTag());

        var record = dbRecord.MapToDomainEntity();
        
        var result = await Repository.UpdateAsync(record);
        
        // Assert
        Assert.False(result.Success);
        ErrorAssert.RelatedEntitiesNotFound(result.Error, record.Tags.Ids.Last());
    }

    [Fact]
    public async Task RemoveAsync_ShouldSuccess_WhenRunningRecordExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbTags = EntitiesFactory.CreateTags(2);
        
        var dbRecord = EntitiesFactory.CreateRunningRecord();
        dbRecord.Activities = dbActivities.ToList();
        dbRecord.Tags = dbTags.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.AddRangeAsync(dbActivities);
        await Database.AddRangeAsync(dbTags);
        await Database.SaveChangesAsync();
        Database.Entry(dbRecord).State = EntityState.Detached;

        // Act
        var record = dbRecord.MapToDomainEntity();
        
        var writeResult = await Repository.DeleteAsync(record.Id);
        
        // Assert
        Assert.True(writeResult.Success);

        var userId = record.UserId.ToString();
        var dbRecordActual = await Database.RunningRecords
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.UserId == userId);
        
        Assert.Null(dbRecordActual);
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnError_WhenEntityIsNotExists()
    {
        // Arrange
        var userId = UserId.Generate();
        var id = RunningRecordId.FromUserId(userId);

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
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbRecord = EntitiesFactory.CreateRunningRecord();
        dbRecord.Activities = dbActivities.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.AddRangeAsync(dbActivities);
        await Database.SaveChangesAsync();
        
        var userId = RunningRecordId.Parse(dbRecord.UserId, null);
        
        // Act
        var result = await Repository.FindByIdAsync(userId);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        
        var record = result.Value;
        
        var actualId = record.Id.ToString();
        var actualUserId = record.UserId.ToString();
        Assert.Equal(dbRecord.UserId, actualId);
        Assert.Equal(dbRecord.UserId, actualUserId);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldFailure_WhenEntityDoesNotExist()
    {
        // Arrange
        var userId = UserId.Generate();
        var id = RunningRecordId.FromUserId(userId);
        
        // Act
        var result = await Repository.FindByIdAsync(id);
        
        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, result.Error.Type);
    }
}