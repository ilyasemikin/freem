using Freem.EFCore.Extensions;
using Freem.Entities.RunningRecords.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories;

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
        
        await Repository.CreateAsync(record);

        // Assert
        var dbRecordActual = await Database.RunningRecords.FirstOrDefaultAsync(e => e.UserId == record.Id);

        Assert.Equal(dbRecord, dbRecordActual);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenRelatedActivityNotExists()
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
        
        var exception = await Record.ExceptionAsync(() => Repository.CreateAsync(record));
        
        // Assert
        Assert.NotNull(exception);
        
        var concreteException = Assert.IsType<NotFoundRelatedException>(exception);
        Assert.Equal(1, concreteException.RelatedIds.Count);
        Assert.Equal(record.Activities.Identifiers.First(), concreteException.RelatedIds[0]);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenTagNotExists()
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
            
        var exception = await Record.ExceptionAsync(() => Repository.CreateAsync(record));
        
        // Assert
        Assert.NotNull(exception);
        
        var concreteException = Assert.IsType<NotFoundRelatedException>(exception);
        Assert.Equal(1, concreteException.RelatedIds.Count);
        Assert.Equal(record.Activities.Identifiers.First(), concreteException.RelatedIds[0]);
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
        Database.DetachEntry(dbRecord);

        // Act
        var dbUpdatedRecord = dbRecord.Clone();
        dbUpdatedRecord.Name = "Updated";
        dbUpdatedRecord.Description = "Updated";
        dbUpdatedRecord.UpdatedAt = DateTimeOffset.UtcNow;

        var record = dbUpdatedRecord.MapToDomainEntity();
        
        await Repository.UpdateAsync(record);
        
        // Assert
        var dbRecordActual = await Database.RunningRecords
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.UserId == record.UserId);
        
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
        await Repository.UpdateAsync(record);
        
        // Assert
        var dbActualRecord = await Database.RunningRecords
            .Include(e => e.Activities)
            .FirstOrDefaultAsync(e => e.UserId == dbUser.Id);
        
        Assert.NotNull(dbActualRecord);
        Assert.Null(dbActualRecord.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenEntityNotExists()
    {
        // Arrange
        var dbRecord = EntitiesFactory.CreateRunningRecord();
        dbRecord.Activities = [EntitiesFactory.CreateActivity()];

        // Act
        var record = dbRecord.MapToDomainEntity();
        
        var exception = await Record.ExceptionAsync(() => Repository.UpdateAsync(record));
        
        // Assert
        Assert.NotNull(exception);
        
        var concreteException = Assert.IsType<NotFoundException>(exception);
        Assert.Equal(record.Id, concreteException.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenRelatedActivityNotExists()
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
        Database.DetachEntry(dbRecord);
        
        // Act
        dbRecord.Activities.Add(EntitiesFactory.CreateActivity());

        var record = dbRecord.MapToDomainEntity();
        
        var exception = await Record.ExceptionAsync(() => Repository.UpdateAsync(record));
        
        // Assert
        Assert.NotNull(exception);
        
        var concreteException = Assert.IsType<NotFoundRelatedException>(exception);
        Assert.Equal(1, concreteException.RelatedIds.Count);
        Assert.Equal(record.Activities.Identifiers.Last(), concreteException.RelatedIds[0]);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenRelatedTagNotExists()
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
        Database.DetachEntry(dbRecord);
        
        // Act
        dbRecord.Tags.Add(EntitiesFactory.CreateTag());

        var record = dbRecord.MapToDomainEntity();
        
        var exception = await Record.ExceptionAsync(() => Repository.UpdateAsync(record));
        
        // Assert
        Assert.NotNull(exception);
        
        var concreteException = Assert.IsType<NotFoundRelatedException>(exception);
        Assert.Equal(1, concreteException.RelatedIds.Count);
        Assert.Equal(record.Tags.Identifiers.Last(), concreteException.RelatedIds[0]);
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
        Database.DetachEntry(dbRecord);

        // Act
        var record = dbRecord.MapToDomainEntity();
        
        await Repository.RemoveAsync(record.Id);
        
        // Assert
        var dbRecordActual = await Database.RunningRecords
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.UserId == record.UserId);
        
        Assert.Null(dbRecordActual);
    }

    [Fact]
    public async Task RemoveAsync_ShouldThrowException_WhenEntityIsNotExists()
    {
        // Arrange
        var id = IdentifiersGenerator.Generate<RunningRecordIdentifier>();

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
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbRecord = EntitiesFactory.CreateRunningRecord();
        dbRecord.Activities = dbActivities.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.AddRangeAsync(dbActivities);
        await Database.SaveChangesAsync();
        
        var userId = (RunningRecordIdentifier)dbRecord.UserId;
        
        // Act
        var result = await Repository.FindByIdAsync(userId);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Founded);
        Assert.NotNull(result.Entity);
        
        var record = result.Entity;
        
        Assert.Equal(dbRecord.UserId, record.Id);
        Assert.Equal(dbRecord.UserId, record.UserId);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldFailure_WhenEntityDoesNotExist()
    {
        // Arrange
        var userId = IdentifiersGenerator.Generate<RunningRecordIdentifier>();
        
        // Act
        var result = await Repository.FindByIdAsync(userId);
        
        // Assert
        Assert.NotNull(result);
        Assert.False(result.Founded);
        Assert.Null(result.Entity);
    }
}