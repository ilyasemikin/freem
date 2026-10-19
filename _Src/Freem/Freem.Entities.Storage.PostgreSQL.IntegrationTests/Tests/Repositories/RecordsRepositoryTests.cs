using Freem.Domain.Records.Identifiers;
using Freem.Domain.Storage.Exceptions;
using Freem.Domain.Storage.Models.Filters.Models;
using Freem.Domain.Storage.Models.Identifiers;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Users.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;
using Freem.Time.Models;
using Microsoft.EntityFrameworkCore;
using Utilities.Collections;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories;

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
        
        await Repository.CreateAsync(record);

        // Assert
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
        await Repository.UpdateAsync(record);
        
        // Assert
        var dbRecordActual = await Database.Records.FindEntityAsync(record.Id);
        
        Assert.NotNull(dbRecordActual);
        Assert.Null(dbRecordActual.UpdatedAt);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenRelatedActivityNotExists()
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
        
        var exception = await Record.ExceptionAsync(() => Repository.CreateAsync(record));
        
        // Assert
        Assert.NotNull(exception);
        
        var concreteException = Assert.IsType<NotFoundRelatedException>(exception);
        Assert.Single(concreteException.RelatedIds);
        Assert.Equal(record.Activities.Identifiers.First(), concreteException.RelatedIds[0]);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenRelatedTagNotExists()
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

        var exception = await Record.ExceptionAsync(() => Repository.CreateAsync(record));
        
        // Assert
        Assert.NotNull(exception);
        
        var concreteException = Assert.IsType<NotFoundRelatedException>(exception);
        Assert.Single(concreteException.RelatedIds);
        Assert.Equal(record.Tags.Identifiers.First(), concreteException.RelatedIds[0]);
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
        
        await Repository.UpdateAsync(record);

        // Assert
        var dbRecordActual = await Database.Records.FindEntityAsync(record.Id);
        
        Assert.NotEqual(dbRecord, dbRecordActual);
        Assert.Equal(dbUpdatedRecord, dbRecordActual);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenEntityNotExists()
    {
        // Arrange
        var dbRecord = EntitiesFactory.CreateRecord();
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
        
        var exception = await Record.ExceptionAsync(() => Repository.UpdateAsync(record));
        
        // Assert
        Assert.NotNull(exception);
        
        var concreteException = Assert.IsType<NotFoundRelatedException>(exception);
        Assert.Single(concreteException.RelatedIds);
        Assert.Equal(record.Activities.Identifiers.Last(), concreteException.RelatedIds[0]);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenRelatedTagNotExists()
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
        
        var exception = await Record.ExceptionAsync(() => Repository.UpdateAsync(record));
        
        // Assert
        Assert.NotNull(exception);
        
        var concreteException = Assert.IsType<NotFoundRelatedException>(exception);
        Assert.Single(concreteException.RelatedIds);
        Assert.Equal(record.Tags.Identifiers.First(), concreteException.RelatedIds[0]);
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

        await Repository.DeleteAsync(record.Id);
        
        // Assert
        var dbRecordActual = await Database.Records.FindEntityAsync(record.Id);
        
        Assert.Null(dbRecordActual);
    }

    [Fact]
    public async Task RemoveAsync_ShouldThrowException_WhenEntityIsNotExists()
    {
        // Arrange
        var id = IdentifiersGenerator.Generate<RecordIdentifier>();
        
        // Act
        var exception = await Record.ExceptionAsync(() => Repository.DeleteAsync(id));
        
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
        var dbTags = EntitiesFactory.CreateTags(2);
        var dbRecord = EntitiesFactory.CreateRecord();
        dbRecord.Activities = dbActivities.ToList();
        dbRecord.Tags = dbTags.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.AddRangeAsync(dbActivities);
        await Database.AddRangeAsync(dbTags);
        await Database.SaveChangesAsync();
        
        // Act
        var id = (RecordIdentifier)dbRecord.Id;
        
        var result = await Repository.FindByIdAsync(id);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Founded);
        Assert.NotNull(result.Entity);

        var record = result.Entity;
        
        Assert.Equal(dbRecord.Id, record.Id);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldFailure_WhenEntityNotExists()
    {
        // Act
        var id = IdentifiersGenerator.Generate<RecordIdentifier>();

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
        var dbActivities = EntitiesFactory.CreateActivities(2);
        var dbRecord = EntitiesFactory.CreateRecord();
        dbRecord.Activities = dbActivities.ToList();
        
        await Database.AddRangeAsync(dbUser, dbRecord);
        await Database.AddRangeAsync(dbActivities);
        await Database.SaveChangesAsync();
        
        var recordId = (RecordIdentifier)dbRecord.Id;
        var userId = (UserIdentifier)dbRecord.UserId;
        
        var ids = new RecordAndUserIdentifiers(recordId, userId);
        
        // Act
        var result = await Repository.FindByMultipleIdAsync(ids);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Founded);
        Assert.NotNull(result.Entity);
        
        var record = result.Entity;
        
        Assert.Equal(dbRecord.Id, record.Id);
    }

    [Fact]
    public async Task FindAsyncByUser_ShouldReturnRelatedIdentifiers()
    {
        // Arrange
        var dbRecord = await AddRecordWithRelationsAsync();

        var userId = (UserIdentifier)dbRecord.UserId;
        var filter = new RecordByUserFilter(userId)
        {
            Limit = new Limit(),
            Offset = new Offset()
        };
        
        // Act
        var result = await Repository.FindAsync(filter);
        var records = await result.ToListAsync();
        
        // Assert
        var record = Assert.Single(records);
        AssertRelatedIdentifiers(dbRecord, record);
    }

    [Fact]
    public async Task FindAsyncByPeriod_ShouldReturnRelatedIdentifiers()
    {
        // Arrange
        var dbRecord = await AddRecordWithRelationsAsync();

        var userId = (UserIdentifier)dbRecord.UserId;
        var startAt = dbRecord.StartAt.AddHours(-1);
        var endAt = dbRecord.EndAt.AddHours(1);
        var period = new DateTimePeriod(startAt, endAt);
        var filter = new RecordByPeriodFilter(userId, period);
        
        // Act
        var result = await Repository.FindAsync(filter);
        var records = await result.ToListAsync();
        
        // Assert
        var record = Assert.Single(records);
        AssertRelatedIdentifiers(dbRecord, record);
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
            ? (RecordIdentifier)dbRecord.Id
            : IdentifiersGenerator.Generate<RecordIdentifier>();
        var userId = userIdExists 
            ? (UserIdentifier)dbUser.Id 
            : GenerateUserIdentifier();
        
        var ids = new RecordAndUserIdentifiers(recordId, userId);
        
        // Act
        var result = await Repository.FindByMultipleIdAsync(ids);
        
        // Assert
        Assert.NotNull(result);
        Assert.False(result.Founded);
        Assert.Null(result.Entity);
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

    private static void AssertRelatedIdentifiers(RecordEntity expected, Domain.Entities.Records.Record actual)
    {
        var expectedActivityIds = expected.Activities!.Select(activity => activity.Id);
        var actualActivityIds = actual.Activities.Identifiers.Select(id => id.ToString());
        Assert.True(expectedActivityIds.UnorderedEquals(actualActivityIds));

        var expectedTagIds = expected.Tags!.Select(tag => tag.Id);
        var actualTagIds = actual.Tags.Identifiers.Select(id => id.ToString());
        Assert.True(expectedTagIds.UnorderedEquals(actualTagIds));
    }
}