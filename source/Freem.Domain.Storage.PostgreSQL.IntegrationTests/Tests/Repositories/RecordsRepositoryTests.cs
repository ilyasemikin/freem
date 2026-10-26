using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Records.Sources;
using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Users;
using Freem.Domain.Users.Models;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;
using Freem.Domain.Storage.PostgreSQL.Database.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Records;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;
using Freem.Time;
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
    public async Task CreateAsync_ShouldStoreImportSource()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivities = EntitiesFactory.CreateActivities(1);

        await Database.AddRangeAsync(dbUser);
        await Database.AddRangeAsync(dbActivities);
        await Database.SaveChangesAsync();

        // Act
        var dbRecord = EntitiesFactory.CreateImportRecord();
        dbRecord.Activities = dbActivities.ToList();
        dbRecord.Tags = [];

        var record = dbRecord.MapToDomainEntity();

        var writeResult = await Repository.CreateAsync(record);

        // Assert
        Assert.True(writeResult.Success);

        var actual = await Repository.FindByIdAsync(record.Id);

        Assert.True(actual.Success);
        Assert.IsType<ImportRecordSource>(actual.Value.Source);
        Assert.Equal(record.Source, actual.Value.Source);
    }

    [Fact]
    public async Task CreateManyAsync_ShouldCreateAllRecords_WhenPassValid()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbTags = EntitiesFactory.CreateTags(2);

        await Database.AddRangeAsync(dbUser);
        await Database.AddRangeAsync(dbActivities);
        await Database.AddRangeAsync(dbTags);
        await Database.SaveChangesAsync();

        var dbRecords = EntitiesFactory.CreateRecords(3);
        foreach (var dbRecord in dbRecords)
        {
            dbRecord.Activities = dbActivities.ToList();
            dbRecord.Tags = dbTags.ToList();
        }

        var records = dbRecords
            .Select(r => r.MapToDomainEntity())
            .ToArray();

        // Act
        var writeResult = await Repository.CreateManyAsync(records);

        // Assert
        Assert.True(writeResult.Success);

        foreach (var dbRecord in dbRecords)
        {
            var record = dbRecord.MapToDomainEntity();
            var dbRecordActual = await Database.Records.FindEntityAsync(record.Id);

            Assert.Equal(dbRecord, dbRecordActual);
        }
    }

    [Fact]
    public async Task CreateManyAsync_ShouldReturnError_WhenRelatedActivityNotExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivity = EntitiesFactory.CreateActivity();

        await Database.AddRangeAsync(dbUser, dbActivity);
        await Database.SaveChangesAsync();

        var validDbRecord = EntitiesFactory.CreateRecord();
        validDbRecord.Activities = [dbActivity];
        var invalidDbRecord = EntitiesFactory.CreateRecord();
        invalidDbRecord.Activities = [EntitiesFactory.CreateActivity()];

        var validRecord = validDbRecord.MapToDomainEntity();
        var invalidRecord = invalidDbRecord.MapToDomainEntity();

        // Act
        var writeResult = await Repository.CreateManyAsync([validRecord, invalidRecord]);

        // Assert
        Assert.False(writeResult.Success);

        Database.ChangeTracker.Clear();
        var count = await Database.Records.CountAsync();
        Assert.Equal(0, count);
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
        var startAt = dbRecord.Periods!.Min(p => p.StartAt).AddHours(-1);
        var endAt = dbRecord.Periods!.Max(p => p.EndAt).AddHours(1);
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

    [Fact]
    public async Task UpdateAsync_ShouldAddChangeAndRemovePeriods()
    {
        // Arrange
        var baseTime = new DateTimeOffset(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);
        var dbRecord = await AddRecordWithPeriodsAsync([(0, 60), (120, 180)]);
        var record = await FindRecordAsync(dbRecord.Id);

        var kept = record.Periods[0];
        var changedOffsetValue = TimeSpan.FromHours(3);
        var changedOffset = new TimeZoneOffset(changedOffsetValue);
        var addedOffsetValue = TimeSpan.FromHours(-5);
        var addedOffset = new TimeZoneOffset(addedOffsetValue);
        var changed = new RecordPeriod(
            kept.Id,
            new DateTimePeriod(baseTime, baseTime.AddMinutes(30)),
            changedOffset,
            RecordPeriodDescription.Parse("changed", null));
        var added = new RecordPeriod(
            RecordPeriodId.Generate(),
            new DateTimePeriod(baseTime.AddMinutes(240), baseTime.AddMinutes(300)),
            addedOffset);

        Assert.True(record.Periods.TryReplace(changed));
        Assert.True(record.Periods.TryRemove(record.Periods[1].Id));
        Assert.True(record.Periods.TryAdd(added));

        // Act
        var result = await Repository.UpdateAsync(record);

        // Assert
        Assert.True(result.Success);

        Database.ChangeTracker.Clear();
        var actual = await FindRecordAsync(dbRecord.Id);
        Assert.Equal([changed, added], actual.Periods);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemovePeriods()
    {
        // Arrange
        var dbRecord = await AddRecordWithPeriodsAsync([(0, 60), (120, 180)]);
        var recordId = RecordId.Parse(dbRecord.Id, null);

        // Act
        var result = await Repository.DeleteAsync(recordId);

        // Assert
        Assert.True(result.Success);
        Assert.Empty(await Database.Set<RecordPeriodEntity>().ToListAsync());
    }

    [Fact]
    public async Task FindAsyncByPeriod_ShouldReturnRecordOnce_WhenSeveralPeriodsMatch()
    {
        // Arrange
        var dbRecord = await AddRecordWithPeriodsAsync([(0, 60), (120, 180)]);

        // Act
        var records = await FindByPeriodAsync(dbRecord, -30, 200);

        // Assert
        Assert.Single(records);
    }

    [Fact]
    public async Task FindAsyncByPeriod_ShouldReturnRecord_WhenPeriodCoversFilter()
    {
        // Arrange
        var dbRecord = await AddRecordWithPeriodsAsync([(0, 600)]);

        // Act
        var records = await FindByPeriodAsync(dbRecord, 100, 200);

        // Assert
        Assert.Single(records);
    }

    [Fact]
    public async Task FindAsyncByPeriod_ShouldReturnRecord_WhenOnlyOnePeriodMatches()
    {
        // Arrange
        var dbRecord = await AddRecordWithPeriodsAsync([(0, 60), (500, 560)]);

        // Act
        var records = await FindByPeriodAsync(dbRecord, 100, 200);

        // Assert
        Assert.Empty(records);

        var matched = await FindByPeriodAsync(dbRecord, 520, 540);
        Assert.Single(matched);
    }

    [Fact]
    public async Task FindAsyncByPeriod_ShouldNotReturnRecord_WhenPeriodOnlyTouchesFilter()
    {
        // Arrange
        var dbRecord = await AddRecordWithPeriodsAsync([(0, 60)]);

        // Act
        var records = await FindByPeriodAsync(dbRecord, 60, 120);

        // Assert
        Assert.Empty(records);
    }

    [Fact]
    public async Task FindAsyncByPeriod_ShouldPaginateByCursor()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbActivity = EntitiesFactory.CreateActivity();
        await Database.AddRangeAsync(dbUser, dbActivity);

        var baseTime = new DateTimeOffset(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);
        var layouts = new (int Start, int End)[][]
        {
            [(0, 10), (400, 410)],
            [(5, 20)],
            [(5, 25)],
            [(5, 25)],
            [(100, 110)]
        };

        foreach (var layout in layouts)
        {
            var dbRecord = EntitiesFactory.CreateRecord();
            dbRecord.Activities = [dbActivity];
            dbRecord.Tags = [];
            dbRecord.Periods = layout
                .Select(p => new RecordPeriodEntity
                {
                    Id = RecordPeriodId.Generate().ToString(),
                    RecordId = dbRecord.Id,
                    StartAt = baseTime.AddMinutes(p.Start),
                    EndAt = baseTime.AddMinutes(p.End)
                })
                .ToList();
            await Database.AddAsync(dbRecord);
        }

        await Database.SaveChangesAsync();
        Database.ChangeTracker.Clear();

        var userId = UserId.Parse(dbUser.Id, null);
        var period = new DateTimePeriod(baseTime.AddMinutes(-10), baseTime.AddMinutes(500));

        // Act
        var all = new List<Domain.Entities.Records.Record>();
        RecordId? after = null;
        while (true)
        {
            var filter = new RecordByPeriodFilter(userId, period, after) { Limit = new Limit(2) };
            var result = await Repository.FindAsync(filter);
            Assert.True(result.Success);
            Assert.Equal(layouts.Length, result.Value.Count);

            var page = await result.Value.ToListAsync();
            if (page.Count == 0)
                break;

            all.AddRange(page);
            after = page[^1].Id;
        }

        // Assert
        Assert.Equal(layouts.Length, all.Count);
        Assert.Equal(layouts.Length, all.Select(r => r.Id).Distinct().Count());

        var keys = all
            .Select(r => (r.Periods.Min(p => p.Period.StartAt), r.Periods.Max(p => p.Period.EndAt), r.Id.ToString()))
            .ToArray();
        Assert.Equal(keys.OrderBy(k => k.Item1).ThenBy(k => k.Item2).ThenBy(k => k.Item3, StringComparer.Ordinal), keys);
    }

    private async Task<RecordEntity> AddRecordWithPeriodsAsync(IEnumerable<(int Start, int End)> periods)
    {
        var baseTime = new DateTimeOffset(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);
        var dbUser = EntitiesFactory.User;
        var dbActivity = EntitiesFactory.CreateActivity();
        var dbRecord = EntitiesFactory.CreateRecord();
        dbRecord.Activities = [dbActivity];
        dbRecord.Tags = [];
        dbRecord.Periods = periods
            .Select(p => new RecordPeriodEntity
            {
                Id = RecordPeriodId.Generate().ToString(),
                RecordId = dbRecord.Id,
                StartAt = baseTime.AddMinutes(p.Start),
                EndAt = baseTime.AddMinutes(p.End)
            })
            .ToList();

        await Database.AddRangeAsync(dbUser, dbActivity, dbRecord);
        await Database.SaveChangesAsync();
        Database.ChangeTracker.Clear();

        return dbRecord;
    }

    private async Task<Domain.Entities.Records.Record> FindRecordAsync(string id)
    {
        var recordId = RecordId.Parse(id, null);
        var result = await Repository.FindByIdAsync(recordId);
        Assert.True(result.Success);
        return result.Value;
    }

    private async Task<List<Domain.Entities.Records.Record>> FindByPeriodAsync(
        RecordEntity dbRecord, int startMinutes, int endMinutes)
    {
        var baseTime = new DateTimeOffset(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);
        var userId = UserId.Parse(dbRecord.UserId, null);
        var period = new DateTimePeriod(baseTime.AddMinutes(startMinutes), baseTime.AddMinutes(endMinutes));
        var result = await Repository.FindAsync(new RecordByPeriodFilter(userId, period));
        Assert.True(result.Success);
        return await result.Value.ToListAsync();
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