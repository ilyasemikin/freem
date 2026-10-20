using Freem.Domain;
using Freem.Domain.Records;
using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Users;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;
using Freem.Domain.Storage.PostgreSQL.Database.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Records;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;
using Freem.Time.Models;
using Microsoft.EntityFrameworkCore;
using Utilities.Collections;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Repositories;

public sealed class RecordsRepositoryTests : BaseRepositoryTests<IRecordRepository>
{
    public RecordsRepositoryTests(
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
        
        // Act
        var dbRecord = EntitiesFactory.CreateRecord();
        dbRecord.Activities = dbActivities.ToList();
        dbRecord.Tags = dbTags.ToList();
        
        var record = dbRecord.MapToDomainEntity();
        
        var writeResult = await Repository.CreateAsync(record);

        // Assert
        Assert.True(writeResult.Success);

        var dbRecordActual = await Database.Records.FindEntityAsync(record.Id);
        
        Assert.Equal(dbRecord, dbRecordActual);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateCreatedEntity_WhenEntityIsNotUpdatedActually()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbRecord = EntitiesFactory.CreateRecord();
        dbRecord.Activities = dbActivities.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.AddRangeAsync(dbActivities);
        await Database.SaveChangesAsync();

        var record = dbRecord.MapToDomainEntity();
        
        // Act
        var writeResult = await Repository.UpdateAsync(record);
        
        // Assert
        Assert.True(writeResult.Success);

        var dbRecordActual = await Database.Records.FindEntityAsync(record.Id);
        
        Assert.NotNull(dbRecordActual);
        Assert.Null(dbRecordActual.UpdatedAt);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnError_WhenRelatedActivityNotExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivity = EntitiesFactory.CreateActivity();
        var dbRecord = EntitiesFactory.CreateRecord();
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
        var dbRecord = EntitiesFactory.CreateRecord();
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
        
        var dbRecord = EntitiesFactory.CreateRecord();
        dbRecord.Activities = dbActivities.ToList();
        dbRecord.Tags = dbTags.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.AddRangeAsync(dbActivities);
        await Database.AddRangeAsync(dbTags);
        await Database.SaveChangesAsync();
        Database.Entry(dbRecord).State = EntityState.Detached;
        
        var dbNewActivity = EntitiesFactory.CreateActivity();
        var dbNewTag = EntitiesFactory.CreateTag();
        await Database.AddRangeAsync(dbNewActivity, dbNewTag);
        await Database.SaveChangesAsync();

        // Act
        var dbUpdatedRecord = dbRecord.Copy();
        dbUpdatedRecord.Name = "Updated";
        dbUpdatedRecord.Description = "Updated";
        dbUpdatedRecord.UpdatedAt = DateTimeOffset.UtcNow;
        dbUpdatedRecord.Activities!.Add(dbNewActivity);
        dbUpdatedRecord.Tags!.Add(dbNewTag);

        var record = dbUpdatedRecord.MapToDomainEntity();
        
        var writeResult = await Repository.UpdateAsync(record);

        // Assert
        Assert.True(writeResult.Success);

        var dbRecordActual = await Database.Records.FindEntityAsync(record.Id);
        
        Assert.NotEqual(dbRecord, dbRecordActual);
        Assert.Equal(dbUpdatedRecord, dbRecordActual);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnError_WhenEntityNotExists()
    {
        // Arrange
        var dbRecord = EntitiesFactory.CreateRecord();
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
        
        var dbRecord = EntitiesFactory.CreateRecord();
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
        
        var dbRecord = EntitiesFactory.CreateRecord();
        dbRecord.Activities = dbActivities.ToList();
        dbRecord.Tags = dbTags.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.AddRangeAsync(dbActivities);
        await Database.AddRangeAsync(dbTags);
        await Database.SaveChangesAsync();
        Database.Entry(dbRecord).State = EntityState.Detached;
        
        // Act
        dbRecord.Tags = [EntitiesFactory.CreateTag()];

        var record = dbRecord.MapToDomainEntity();
        
        var result = await Repository.UpdateAsync(record);
        
        // Assert
        Assert.False(result.Success);
        ErrorAssert.RelatedEntitiesNotFound(result.Error, record.Tags.Ids.First());
    }
    
    [Fact]
    public async Task RemoveAsync_ShouldSuccess_WhenRecordExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbTags = EntitiesFactory.CreateTags(2);
        
        var dbRecord = EntitiesFactory.CreateRecord();
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

        var dbRecordActual = await Database.Records.FindEntityAsync(record.Id);
        
        Assert.Null(dbRecordActual);
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnError_WhenEntityIsNotExists()
    {
        // Arrange
        var id = RecordId.Generate();
        
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
        var dbTags = EntitiesFactory.CreateTags(2);
        var dbRecord = EntitiesFactory.CreateRecord();
        dbRecord.Activities = dbActivities.ToList();
        dbRecord.Tags = dbTags.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.AddRangeAsync(dbActivities);
        await Database.AddRangeAsync(dbTags);
        await Database.SaveChangesAsync();
        
        // Act
        var id = RecordId.Parse(dbRecord.Id, null);
        
        var result = await Repository.FindByIdAsync(id);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Value);

        var record = result.Value;
        
        var actualId = record.Id.ToString();
        Assert.Equal(dbRecord.Id, actualId);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldFailure_WhenEntityNotExists()
    {
        // Act
        var id = RecordId.Generate();

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
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbRecord = EntitiesFactory.CreateRecord();
        dbRecord.Activities = dbActivities.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.AddRangeAsync(dbActivities);
        await Database.SaveChangesAsync();
        
        var recordId = RecordId.Parse(dbRecord.Id, null);
        var userId = UserId.Parse(dbRecord.UserId, null);
        
        var ids = new RecordAndUserIds(recordId, userId);
        
        // Act
        var result = await Repository.FindByMultipleIdAsync(ids);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Value);
        
        var record = result.Value;
        
        var actualId = record.Id.ToString();
        Assert.Equal(dbRecord.Id, actualId);
    }

    [Fact]
    public async Task FindAsyncByUser_ShouldReturnRelatedIds()
    {
        // Arrange
        var dbRecord = await AddRecordWithRelationsAsync();

        var userId = UserId.Parse(dbRecord.UserId, null);
        var filter = new RecordByUserFilter(userId)
        {
            Limit = new Limit(),
            Offset = new Offset()
        };
        
        // Act
        var result = await Repository.FindAsync(filter);
        
        // Assert
        Assert.True(result.Success);

        var records = await result.Value.ToListAsync();
        var record = Assert.Single(records);
        AssertRelatedIds(dbRecord, record);
    }

    [Fact]
    public async Task FindAsyncByPeriod_ShouldReturnRelatedIds()
    {
        // Arrange
        var dbRecord = await AddRecordWithRelationsAsync();

        var userId = UserId.Parse(dbRecord.UserId, null);
        var startAt = dbRecord.StartAt.AddHours(-1);
        var endAt = dbRecord.EndAt.AddHours(1);
        var period = new DateTimePeriod(startAt, endAt);
        var filter = new RecordByPeriodFilter(userId, period);
        
        // Act
        var result = await Repository.FindAsync(filter);
        
        // Assert
        Assert.True(result.Success);

        var records = await result.Value.ToListAsync();
        var record = Assert.Single(records);
        AssertRelatedIds(dbRecord, record);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public async Task FindAsync_ShouldFailure_WhenPartOfIdsNotExists(bool recordIdExists, bool userIdExists)
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbRecord = EntitiesFactory.CreateRecord();
        dbRecord.Activities = dbActivities.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.AddRangeAsync(dbActivities);
        await Database.SaveChangesAsync();
        
        var recordId = recordIdExists
            ? RecordId.Parse(dbRecord.Id, null)
            : RecordId.Generate();
        var userId = userIdExists 
            ? UserId.Parse(dbUser.Id, null) 
            : GenerateUserId();
        
        var ids = new RecordAndUserIds(recordId, userId);
        
        // Act
        var result = await Repository.FindByMultipleIdAsync(ids);
        
        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, result.Error.Type);
    }

    private async Task<RecordEntity> AddRecordWithRelationsAsync()
    {
        var dbUser = EntitiesFactory.User;
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbTags = EntitiesFactory.CreateTags(2);
        var dbRecord = EntitiesFactory.CreateRecord();
        dbRecord.Activities = dbActivities.ToList();
        dbRecord.Tags = dbTags.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.AddRangeAsync(dbActivities);
        await Database.AddRangeAsync(dbTags);
        await Database.SaveChangesAsync();
        
        Database.ChangeTracker.Clear();

        return dbRecord;
    }

    private static void AssertRelatedIds(RecordEntity expected, Domain.Entities.Records.Record actual)
    {
        var expectedActivityIds = expected.Activities!.Select(activity => activity.Id);
        var actualActivityIds = actual.Activities.Ids.Select(id => id.ToString());
        Assert.True(expectedActivityIds.UnorderedEquals(actualActivityIds));

        var expectedTagIds = expected.Tags!.Select(tag => tag.Id);
        var actualTagIds = actual.Tags.Ids.Select(id => id.ToString());
        Assert.True(expectedTagIds.UnorderedEquals(actualTagIds));
    }
}