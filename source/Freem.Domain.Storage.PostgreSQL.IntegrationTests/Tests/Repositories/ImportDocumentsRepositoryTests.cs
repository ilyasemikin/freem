using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Entities.Import;
using Freem.Domain.Formats;
using Freem.Domain.Import;
using Freem.Domain.Records.Models;
using Freem.Domain.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure.Extensions;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;
using Freem.Domain.Storage.Repositories.Imports;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Domain.Users;
using Freem.Domain.Users.Models;
using Freem.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Repositories;

public sealed class ImportDocumentsRepositoryTests : BaseRepositoryTests<IImportDocumentRepository>
{
    private static readonly DateTimeOffset BaseTime = new(2026, 10, 1, 8, 0, 0, TimeSpan.Zero);

    private readonly ITestOutputHelper _output;

    public ImportDocumentsRepositoryTests(ITestOutputHelper output)
        : base(output)
    {
        _output = output;
    }

    private async Task<UserId> CreateUserAsync()
    {
        var user = EntitiesFactory.User;
        await Database.Users.AddAsync(user);
        await Database.SaveChangesAsync();
        return UserId.Parse(user.Id, null);
    }

    private static ImportDocument CreateDocument(
        UserId userId,
        ImportDocumentStatus? status = null,
        int rowsInFile = 12,
        int rowsParsed = 10)
    {
        var id = ImportDocumentId.Generate();
        var format = FormatKey.Parse("time-tracker", null);
        var fileName = ImportFileName.Parse("All.csv", null);
        var offsetValue = TimeSpan.FromHours(3);
        var timeZoneOffset = new TimeZoneOffset(offsetValue);
        return new ImportDocument(
            id,
            userId,
            format,
            fileName,
            timeZoneOffset,
            status ?? ImportDocumentStatus.Created,
            rowsInFile,
            rowsParsed);
    }

    private async Task<ImportDocument> CreateDocumentAsync(
        UserId userId,
        IReadOnlyList<ImportDocumentTag>? tags = null,
        IReadOnlyList<ImportDocumentActivity>? activities = null)
    {
        var document = CreateDocument(userId);
        var createResult = await Repository.CreateAsync(document);
        Assert.True(createResult.Success);
        var addResult = await Repository.AddTagsAndActivitiesAsync(document.Id, tags ?? [], activities ?? []);
        Assert.True(addResult.Success);
        return document;
    }

    private async Task<ImportDocumentActivity[]> FindActivitiesAsync(ImportDocumentId documentId)
    {
        var filter = new ImportDocumentActivityByDocumentFilter(documentId) { Limit = 100, Offset = 0 };
        var result = await Repository.FindActivitiesAsync(filter);
        Assert.True(result.Success);
        return await result.Value.ToArrayAsync();
    }

    private async Task<ImportDocumentTag[]> FindTagsAsync(ImportDocumentId documentId)
    {
        var filter = new ImportDocumentTagByDocumentFilter(documentId) { Limit = 100, Offset = 0 };
        var result = await Repository.FindTagsAsync(filter);
        Assert.True(result.Success);
        return await result.Value.ToArrayAsync();
    }

    private static ImportTag CreateImportTag(string key, string name)
    {
        var id = ImportTagId.Parse(key, null);
        var tagName = TagName.Parse(name, null);
        return new ImportTag(id, tagName);
    }

    private static ImportActivity CreateImportActivity(string key, string name, params string[] tagKeys)
    {
        var id = ImportActivityId.Parse(key, null);
        var activityName = ActivityName.Parse(name, null);
        var color = ActivityColor.Parse("#6366f1", null);
        var tagIds = tagKeys
            .Select(k => ImportTagId.Parse(k, null))
            .ToList();
        return new ImportActivity(id, activityName, color, ActivityStatus.Active, tagIds);
    }

    private static ImportRow CreateRow(
        ImportDocumentId documentId,
        int index,
        string activityKey = "a1",
        int periods = 1)
    {
        var recordId = ImportRecordId.Parse($"r{index}", null);
        var name = RecordName.Parse($"Row {index}", null);
        ImportActivityId[] activityIds = [ImportActivityId.Parse(activityKey, null)];
        ImportTagId[] tagIds = [ImportTagId.Parse("t1", null)];

        var periodList = new List<ImportRecordPeriod>();
        for (var i = 0; i < periods; i++)
        {
            var start = BaseTime.AddHours(index).AddMinutes(i * 10);
            var period = new DateTimePeriod(start, start.AddMinutes(5));
            var description = RecordPeriodDescription.Parse($"Period {i}", null);
            periodList.Add(new ImportRecordPeriod(period, TimeZoneOffset.Default, description));
        }

        var record = new ImportRecord(recordId, name, null, activityIds, tagIds, periodList);
        var id = ImportRowId.Generate();
        return new ImportRow(id, documentId, record);
    }

    private static ImportRowByDocumentFilter CreateFilter(ImportDocumentId documentId, int limit, int offset)
    {
        return new ImportRowByDocumentFilter(documentId)
        {
            Limit = limit,
            Offset = offset
        };
    }

    [Fact]
    public async Task CreateAsync_ShouldSaveDocument_WhenPassValid()
    {
        var userId = await CreateUserAsync();
        var document = CreateDocument(userId);

        var writeResult = await Repository.CreateAsync(document);
        var readResult = await Repository.FindByIdAsync(document.Id);

        Assert.True(writeResult.Success);
        Assert.True(readResult.Success);
        var actual = readResult.Value;
        Assert.Equal(userId, actual.UserId);
        Assert.Equal(document.Format, actual.Format);
        Assert.Equal(document.FileName, actual.FileName);
        Assert.Equal(document.TimeZoneOffset, actual.TimeZoneOffset);
        Assert.Equal(ImportDocumentStatus.Created, actual.Status);
        Assert.Equal(12, actual.RowsInFile);
        Assert.Equal(10, actual.RowsParsed);
    }

    [Fact]
    public async Task FindActivitiesAsync_ShouldReturnActivitiesWithTagsAndMappings()
    {
        var userId = await CreateUserAsync();
        var dbActivity = EntitiesFactory.CreateActivity();
        await Database.Activities.AddAsync(dbActivity);
        await Database.SaveChangesAsync();
        var activityId = ActivityId.Parse(dbActivity.Id, null);
        var importTag = CreateImportTag("t1", "Work");
        ImportDocumentTag[] tags = [new(importTag)];
        var importActivity = CreateImportActivity("a1", "Coding", "t1");
        ImportDocumentActivity[] activities = [new(importActivity, activityId)];
        var document = await CreateDocumentAsync(userId, tags, activities);

        var actualActivities = await FindActivitiesAsync(document.Id);
        var actualTags = await FindTagsAsync(document.Id);

        var actualTag = Assert.Single(actualTags);
        Assert.Null(actualTag.MappedTagId);
        var actualActivity = Assert.Single(actualActivities);
        Assert.Equal(importActivity.Id, actualActivity.Activity.Id);
        Assert.Equal(importActivity.TagIds, actualActivity.Activity.TagIds);
        Assert.Equal(activityId, actualActivity.MappedActivityId);
    }

    [Fact]
    public async Task FindActivitiesAsync_ShouldPageAndReturnOnlyActivitiesOfDocument()
    {
        var userId = await CreateUserAsync();
        ImportDocumentActivity[] activities =
        [
            new(CreateImportActivity("a1", "First")),
            new(CreateImportActivity("a2", "Second")),
            new(CreateImportActivity("a3", "Third"))
        ];
        var document = await CreateDocumentAsync(userId, activities: activities);
        var otherActivities = new ImportDocumentActivity[] { new(CreateImportActivity("a1", "Other")) };
        await CreateDocumentAsync(userId, activities: otherActivities);
        var filter = new ImportDocumentActivityByDocumentFilter(document.Id) { Limit = 2, Offset = 1 };

        var result = await Repository.FindActivitiesAsync(filter);

        Assert.True(result.Success);
        Assert.Equal(3, result.Value.Count);
        var items = await result.Value.ToArrayAsync();
        Assert.Equal(["a2", "a3"], items.Select(i => i.Activity.Id.ToString()).ToArray());
    }

    [Fact]
    public async Task FindByMultipleIdAsync_ShouldReturnNotFound_WhenUserIsAnother()
    {
        var userId = await CreateUserAsync();
        var document = CreateDocument(userId);
        await Repository.CreateAsync(document);
        var otherUserId = UserId.Generate();
        var ids = new ImportDocumentAndUserIds(document.Id, otherUserId);

        var result = await Repository.FindByMultipleIdAsync(ids);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, result.Error.Type);
    }

    [Fact]
    public async Task FindAsync_ShouldReturnPageAndTotalCount()
    {
        var userId = await CreateUserAsync();
        for (var i = 0; i < 3; i++)
            await Repository.CreateAsync(CreateDocument(userId));
        var filter = new ImportDocumentByUserFilter(userId)
        {
            Limit = 2,
            Offset = 0
        };

        var result = await Repository.FindAsync(filter);

        Assert.True(result.Success);
        Assert.Equal(3, result.Value.Count);
        var items = await result.Value.ToArrayAsync();
        Assert.Equal(2, items.Length);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveDocumentWithRows_AndFailForUnknownId()
    {
        var userId = await CreateUserAsync();
        var document = CreateDocument(userId);
        await Repository.CreateAsync(document);
        await Repository.AddRowsAsync([CreateRow(document.Id, 1)]);

        var deleteResult = await Repository.DeleteAsync(document.Id);
        var secondDeleteResult = await Repository.DeleteAsync(document.Id);

        Assert.True(deleteResult.Success);
        Assert.False(secondDeleteResult.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, secondDeleteResult.Error.Type);
        Assert.Equal(0, await Database.ImportRows.CountAsync());
    }

    [Fact]
    public async Task DeleteUser_ShouldRemoveDocumentsAndRows()
    {
        var userId = await CreateUserAsync();
        var document = CreateDocument(userId);
        await Repository.CreateAsync(document);
        await Repository.AddRowsAsync([CreateRow(document.Id, 1)]);

        await Database.Users.ExecuteDeleteAsync();

        Assert.Equal(0, await Database.ImportDocuments.CountAsync());
        Assert.Equal(0, await Database.ImportRows.CountAsync());
        Assert.Equal(0, await Database.ImportRowPeriods.CountAsync());
    }

    [Fact]
    public async Task ChangeStatusAsync_ShouldChangeStatus_WhenExpectedMatches()
    {
        var userId = await CreateUserAsync();
        var document = CreateDocument(userId);
        await Repository.CreateAsync(document);

        var result = await Repository.ChangeStatusAsync(
            document.Id,
            ImportDocumentStatus.Created,
            ImportDocumentStatus.Parsing);

        Assert.True(result.Success);
        var actual = await Repository.FindByIdAsync(document.Id);
        Assert.True(actual.Success);
        Assert.Equal(ImportDocumentStatus.Parsing, actual.Value.Status);
    }

    [Fact]
    public async Task ChangeStatusAsync_ShouldFail_WhenSecondTransitionFromSameStatus()
    {
        var userId = await CreateUserAsync();
        var document = CreateDocument(userId);
        await Repository.CreateAsync(document);
        await Repository.ChangeStatusAsync(document.Id, ImportDocumentStatus.Created, ImportDocumentStatus.Parsing);

        var result = await Repository.ChangeStatusAsync(
            document.Id,
            ImportDocumentStatus.Created,
            ImportDocumentStatus.Parsing);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.ImportInvalidStatus, result.Error.Type);
    }

    [Fact]
    public async Task ChangeStatusAsync_ShouldFail_WhenTransitionIsNotAllowed()
    {
        var userId = await CreateUserAsync();
        var document = CreateDocument(userId);
        await Repository.CreateAsync(document);

        var result = await Repository.ChangeStatusAsync(
            document.Id,
            ImportDocumentStatus.Created,
            ImportDocumentStatus.Parsed);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.ImportInvalidStatus, result.Error.Type);
        var actual = await Repository.FindByIdAsync(document.Id);
        Assert.True(actual.Success);
        Assert.Equal(ImportDocumentStatus.Created, actual.Value.Status);
    }

    [Fact]
    public async Task ChangeStatusAsync_ShouldReturnNotFound_WhenDocumentDoesNotExist()
    {
        var id = ImportDocumentId.Generate();

        var result = await Repository.ChangeStatusAsync(
            id,
            ImportDocumentStatus.Created,
            ImportDocumentStatus.Parsing);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, result.Error.Type);
    }

    [Fact]
    public async Task AddTagsAndActivitiesAsync_ShouldSaveTagsAndActivities()
    {
        var userId = await CreateUserAsync();
        var document = CreateDocument(userId, ImportDocumentStatus.Parsing);
        await Repository.CreateAsync(document);
        var importTag = CreateImportTag("t1", "Work");
        ImportDocumentTag[] tags = [new(importTag)];
        var firstImportActivity = CreateImportActivity("a1", "Coding", "t1");
        var secondImportActivity = CreateImportActivity("a2", "Reading");
        ImportDocumentActivity[] activities = [new(firstImportActivity), new(secondImportActivity)];

        var result = await Repository.AddTagsAndActivitiesAsync(document.Id, tags, activities);

        Assert.True(result.Success);
        var actualTags = await FindTagsAsync(document.Id);
        var actualActivities = await FindActivitiesAsync(document.Id);
        Assert.Single(actualTags);
        Assert.Equal(2, actualActivities.Length);
        Assert.Equal(firstImportActivity.TagIds, actualActivities[0].Activity.TagIds);
    }

    [Fact]
    public async Task UpdateActivityMappingAsync_ShouldSaveAndRemoveMapping()
    {
        var userId = await CreateUserAsync();
        var dbActivity = EntitiesFactory.CreateActivity();
        await Database.Activities.AddAsync(dbActivity);
        await Database.SaveChangesAsync();
        var activityId = ActivityId.Parse(dbActivity.Id, null);

        var importActivity = CreateImportActivity("a1", "Coding");
        ImportDocumentActivity[] activities = [new(importActivity)];
        var document = await CreateDocumentAsync(userId, activities: activities);

        var mapResult = await Repository.UpdateActivityMappingAsync(document.Id, importActivity.Id, activityId);
        var afterMap = await FindActivitiesAsync(document.Id);
        var removeResult = await Repository.UpdateActivityMappingAsync(document.Id, importActivity.Id, null);
        var afterRemove = await FindActivitiesAsync(document.Id);

        Assert.True(mapResult.Success);
        Assert.Equal(activityId, afterMap[0].MappedActivityId);
        Assert.True(removeResult.Success);
        Assert.Null(afterRemove[0].MappedActivityId);
    }

    [Fact]
    public async Task UpdateActivityMappingAsync_ShouldFail_WhenActivityDoesNotExist()
    {
        var userId = await CreateUserAsync();
        var importActivity = CreateImportActivity("a1", "Coding");
        ImportDocumentActivity[] activities = [new(importActivity)];
        var document = await CreateDocumentAsync(userId, activities: activities);
        var missingId = ActivityId.Generate();

        var result = await Repository.UpdateActivityMappingAsync(document.Id, importActivity.Id, missingId);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, result.Error.Type);
    }

    [Fact]
    public async Task UpdateActivityMappingAsync_ShouldRemoveMapping_WhenMappedActivityIsRemoved()
    {
        var userId = await CreateUserAsync();
        var dbActivity = EntitiesFactory.CreateActivity();
        await Database.Activities.AddAsync(dbActivity);
        await Database.SaveChangesAsync();
        var activityId = ActivityId.Parse(dbActivity.Id, null);
        var importActivity = CreateImportActivity("a1", "Coding");
        ImportDocumentActivity[] activities = [new(importActivity, activityId)];
        var document = await CreateDocumentAsync(userId, activities: activities);

        await Database.Activities.ExecuteDeleteAsync();

        var actual = await FindActivitiesAsync(document.Id);
        Assert.Null(actual[0].MappedActivityId);
    }

    [Fact]
    public async Task UpdateTagMappingAsync_ShouldSaveMappingAndFailForUnknownImportTag()
    {
        var userId = await CreateUserAsync();
        var dbTag = EntitiesFactory.CreateTag();
        await Database.Tags.AddAsync(dbTag);
        await Database.SaveChangesAsync();
        var tagId = TagId.Parse(dbTag.Id, null);
        var importTag = CreateImportTag("t1", "Work");
        ImportDocumentTag[] tags = [new(importTag)];
        var document = await CreateDocumentAsync(userId, tags: tags);
        var unknownImportTagId = ImportTagId.Parse("unknown", null);

        var result = await Repository.UpdateTagMappingAsync(document.Id, importTag.Id, tagId);
        var unknownResult = await Repository.UpdateTagMappingAsync(document.Id, unknownImportTagId, tagId);

        Assert.True(result.Success);
        var actual = await FindTagsAsync(document.Id);
        Assert.Equal(tagId, actual[0].MappedTagId);
        Assert.False(unknownResult.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, unknownResult.Error.Type);
    }

    [Fact]
    public async Task AddRowsAsync_ShouldSaveRowWithPeriodsActivitiesAndTags()
    {
        var userId = await CreateUserAsync();
        var document = CreateDocument(userId);
        await Repository.CreateAsync(document);
        var row = CreateRow(document.Id, 1, periods: 3);

        var writeResult = await Repository.AddRowsAsync([row]);
        var readResult = await Repository.FindRowByIdAsync(row.Id);

        Assert.True(writeResult.Success);
        Assert.True(readResult.Success);
        var actual = readResult.Value;
        Assert.Equal(row.Record.Id, actual.Record.Id);
        Assert.Equal(row.Record.Name, actual.Record.Name);
        Assert.Equal(row.Record.ActivityIds, actual.Record.ActivityIds);
        Assert.Equal(row.Record.TagIds, actual.Record.TagIds);
        Assert.Equal(3, actual.Record.Periods.Count);
        for (var i = 0; i < 3; i++)
        {
            Assert.Equal(row.Record.Periods[i].Period, actual.Record.Periods[i].Period);
            Assert.Equal(row.Record.Periods[i].Description, actual.Record.Periods[i].Description);
        }

    }

    [Fact]
    public async Task AddRowsAsync_ShouldFail_WhenDocumentDoesNotExist()
    {
        var documentId = ImportDocumentId.Generate();

        var result = await Repository.AddRowsAsync([CreateRow(documentId, 1)]);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, result.Error.Type);
    }

    [Fact]
    public async Task FindRowsAsync_ShouldPageManyRowsByStartTime()
    {
        var userId = await CreateUserAsync();
        var document = CreateDocument(userId);
        await Repository.CreateAsync(document);
        const int count = 1500;
        var rows = new List<ImportRow>();
        for (var i = 0; i < count; i++)
        {
            var activityKey = i % 3 == 0 ? "a1" : "a2";
            rows.Add(CreateRow(document.Id, i, activityKey, periods: 2));
        }

        var writeResult = await Repository.AddRowsAsync(rows);
        var secondPage = await Repository.FindRowsAsync(CreateFilter(document.Id, 100, 100));
        var otherDocumentId = ImportDocumentId.Generate();
        var otherPage = await Repository.FindRowsAsync(CreateFilter(otherDocumentId, 100, 0));

        Assert.True(writeResult.Success);
        Assert.True(secondPage.Success);
        Assert.Equal(count, secondPage.Value.Count);
        var items = await secondPage.Value.ToArrayAsync();
        Assert.Equal(100, items.Length);
        Assert.Equal("r100", items[0].Record.Id.ToString());
        Assert.Equal("r199", items[99].Record.Id.ToString());
        Assert.All(items, i => Assert.Equal(2, i.Record.Periods.Count));
        Assert.True(otherPage.Success);
        Assert.Equal(0, otherPage.Value.Count);
    }

    [Fact]
    public async Task FindRowsAsync_ShouldReturnOnlyRowsWithGivenIds_WhenIdsAreSet()
    {
        var userId = await CreateUserAsync();
        var document = CreateDocument(userId);
        var otherDocument = CreateDocument(userId);
        await Repository.CreateAsync(document);
        await Repository.CreateAsync(otherDocument);
        var rows = Enumerable
            .Range(0, 5)
            .Select(i => CreateRow(document.Id, i))
            .ToArray();
        var otherRow = CreateRow(otherDocument.Id, 10);
        await Repository.AddRowsAsync(rows);
        await Repository.AddRowsAsync([otherRow]);
        ImportRowId[] ids = [rows[3].Id, rows[1].Id, otherRow.Id];
        var filter = new ImportRowByDocumentFilter(document.Id) { Limit = 100, Offset = 0, Ids = ids };

        var result = await Repository.FindRowsAsync(filter);

        Assert.True(result.Success);
        Assert.Equal(2, result.Value.Count);
        var items = await result.Value.ToArrayAsync();
        Assert.Equal(["r1", "r3"], items.Select(i => i.Record.Id.ToString()).ToArray());
    }

    [Fact]
    public async Task AddRowsAsync_ShouldSaveRow_WhenPeriodStartEqualsEnd()
    {
        var userId = await CreateUserAsync();
        var document = CreateDocument(userId);
        await Repository.CreateAsync(document);
        var recordId = ImportRecordId.Parse("r1", null);
        ImportActivityId[] activityIds = [ImportActivityId.Parse("a1", null)];
        ImportTagId[] tagIds = [];
        var period = new DateTimePeriod(BaseTime, BaseTime);
        ImportRecordPeriod[] periods = [new(period, TimeZoneOffset.Default)];
        var record = new ImportRecord(recordId, null, null, activityIds, tagIds, periods);
        var row = new ImportRow(ImportRowId.Generate(), document.Id, record);

        var writeResult = await Repository.AddRowsAsync([row]);
        var readResult = await Repository.FindRowByIdAsync(row.Id);

        Assert.True(writeResult.Success);
        Assert.True(readResult.Success);
        Assert.Equal(period, readResult.Value.Record.Periods[0].Period);
    }

    [Fact]
    public async Task AddRowsAsync_ShouldKeepOrderOfActivitiesAndTags()
    {
        var userId = await CreateUserAsync();
        var document = CreateDocument(userId);
        await Repository.CreateAsync(document);
        var recordId = ImportRecordId.Parse("r1", null);
        ImportActivityId[] activityIds =
        [
            ImportActivityId.Parse("c", null),
            ImportActivityId.Parse("a", null),
            ImportActivityId.Parse("b", null)
        ];
        ImportTagId[] tagIds =
        [
            ImportTagId.Parse("z", null),
            ImportTagId.Parse("y", null)
        ];
        var start = BaseTime;
        var period = new DateTimePeriod(start, start.AddMinutes(5));
        ImportRecordPeriod[] periods = [new(period, TimeZoneOffset.Default)];
        var record = new ImportRecord(recordId, null, null, activityIds, tagIds, periods);
        var row = new ImportRow(ImportRowId.Generate(), document.Id, record);
        await Repository.AddRowsAsync([row]);

        var result = await Repository.FindRowByIdAsync(row.Id);

        Assert.True(result.Success);
        Assert.Equal(activityIds, result.Value.Record.ActivityIds);
        Assert.Equal(tagIds, result.Value.Record.TagIds);
    }

    [Fact]
    public async Task ChangeStatusAsync_ShouldSucceedOnlyOnce_WhenCalledConcurrentlyFromTwoConnections()
    {
        var userId = await CreateUserAsync();
        var document = CreateDocument(userId);
        await Repository.CreateAsync(document);

        var configuration = TestsConfiguration
            .Read()
            .ToStorageConfiguration(_output.WriteLine);
        var services = new ServiceCollection();
        services.AddPostgreSqlStorage(configuration);
        await using var provider = services.BuildServiceProvider();
        var otherRepository = provider.GetRequiredService<IImportDocumentRepository>();

        var results = await Task.WhenAll(
            Repository.ChangeStatusAsync(document.Id, ImportDocumentStatus.Created, ImportDocumentStatus.Parsing),
            otherRepository.ChangeStatusAsync(document.Id, ImportDocumentStatus.Created, ImportDocumentStatus.Parsing));

        Assert.Equal(1, results.Count(r => r.Success));
        var failed = results.Single(r => !r.Success);
        Assert.False(failed.Success);
        Assert.Equal(DomainErrorCatalog.Types.ImportInvalidStatus, failed.Error.Type);
    }
}
