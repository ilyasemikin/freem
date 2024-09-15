﻿using Freem.EFCore.Extensions;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
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
        var dbRecordActual = await Database.RunningRecords.FindAsync(record.Id.Value);
        var dbEvent = await Database.Events.FindEventAsync<RunningRecordEventEntity>(e => e.UserId == record.UserId.Value);

        Assert.Equal(dbRecord, dbRecordActual);
        Assert.NotNull(dbEvent);
        Assert.Equal(EventAction.Created, dbEvent.Action);
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
        
        var exception = await Xunit.Record.ExceptionAsync(() => Repository.CreateAsync(record));
        
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
            
        var exception = await Xunit.Record.ExceptionAsync(() => Repository.CreateAsync(record));
        
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
            .FirstOrDefaultAsync(e => e.UserId == record.UserId.Value);
        var dbEvent = await Database.Events.FindEventAsync<RunningRecordEventEntity>(e => e.UserId == record.UserId.Value);
        
        Assert.NotEqual(dbRecord, dbRecordActual);
        Assert.Equal(dbUpdatedRecord, dbRecordActual);
        Assert.NotNull(dbEvent);
        Assert.Equal(EventAction.Updated, dbEvent.Action);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenEntityNotExists()
    {
        // Arrange
        var dbRecord = EntitiesFactory.CreateRunningRecord();
        dbRecord.Activities = [EntitiesFactory.CreateActivity()];

        // Act
        var record = dbRecord.MapToDomainEntity();
        
        var exception = await Xunit.Record.ExceptionAsync(() => Repository.UpdateAsync(record));
        
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
        
        var exception = await Xunit.Record.ExceptionAsync(() => Repository.UpdateAsync(record));
        
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
        
        var exception = await Xunit.Record.ExceptionAsync(() => Repository.UpdateAsync(record));
        
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
            .FirstOrDefaultAsync(e => e.UserId == record.UserId.Value);
        var dbEvent = await Database.Events.FindEventAsync<RunningRecordEventEntity>(e => e.UserId == record.UserId.Value);
        
        Assert.Null(dbRecordActual);
        Assert.NotNull(dbEvent);
        Assert.Equal(EventAction.Removed, dbEvent.Action);
    }

    [Fact]
    public async Task RemoveAsync_ShouldThrowException_WhenEntityIsNotExists()
    {
        // Arrange
        var idValue = Guid.NewGuid().ToString();
        var id = new UserIdentifier(idValue);

        // Act
        var exception = await Xunit.Record.ExceptionAsync(() => Repository.RemoveAsync(id));
        
        // Assert
        Assert.NotNull(exception);
        
        var concreteException = Assert.IsType<NotFoundException>(exception);
        Assert.Equal(id, concreteException.Id);
    }
}