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
using Freem.Time.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Utilities.Errors;
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
        IReadOnlyList<ImportDocumentTag>? tags = null,
        IReadOnlyList<ImportDocumentActivity>? activities = null)
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
            tags ?? [],
            activities ?? []);
    }

    private static ImportTag CreateSourceTag(string key, string name)
    {
        var id = ImportTagId.Parse(key, null);
        var tagName = TagName.Parse(name, null);
        return new ImportTag(id, tagName);
    }

    private static ImportActivity CreateSourceActivity(string key, string name, params string[] tagKeys)
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
    public async Task CreateAsync_ShouldSaveDocumentWithSource_WhenPassValid()
    {
        var userId = await CreateUserAsync();
        var tagSource = CreateSourceTag("t1", "Work");
        ImportDocumentTag[] tags = [new(tagSource)];
        var activitySource = CreateSourceActivity("a1", "Coding", "t1");
        ImportDocumentActivity[] activities = [new(activitySource, ImportActivityResolution.CreateNew)];
        var document = CreateDocument(userId, tags: tags, activities: activities);

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
        Assert.Single(actual.Tags);
        Assert.Null(actual.Tags[0].Resolution);
        Assert.Single(actual.Activities);
        Assert.Equal(activitySource.Id, actual.Activities[0].Source.Id);
        Assert.Equal(activitySource.TagIds, actual.Activities[0].Source.TagIds);
        Assert.Equal(ImportActivityResolution.CreateNew, actual.Activities[0].Resolution);
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
    public async Task AddSourceAsync_ShouldSaveTagsAndActivities()
    {
        var userId = await CreateUserAsync();
        var document = CreateDocument(userId, ImportDocumentStatus.Parsing);
        await Repository.CreateAsync(document);
        var tagSource = CreateSourceTag("t1", "Work");
        ImportDocumentTag[] tags = [new(tagSource)];
        var firstSource = CreateSourceActivity("a1", "Coding", "t1");
        var secondSource = CreateSourceActivity("a2", "Reading");
        ImportDocumentActivity[] activities = [new(firstSource), new(secondSource)];

        var result = await Repository.AddSourceAsync(document.Id, tags, activities);

        Assert.True(result.Success);
        var actual = await Repository.FindByIdAsync(document.Id);
        Assert.True(actual.Success);
        Assert.Single(actual.Value.Tags);
        Assert.Equal(2, actual.Value.Activities.Count);
        Assert.Equal(firstSource.TagIds, actual.Value.Activities[0].Source.TagIds);
    }

    [Fact]
    public async Task SetActivityResolutionAsync_ShouldSaveExistingCreateNewAndReset()
    {
        var userId = await CreateUserAsync();
        var dbActivity = EntitiesFactory.CreateActivity();
        await Database.Activities.AddAsync(dbActivity);
        await Database.SaveChangesAsync();
        var activityId = ActivityId.Parse(dbActivity.Id, null);

        var source = CreateSourceActivity("a1", "Coding");
        ImportDocumentActivity[] activities = [new(source)];
        var document = CreateDocument(userId, activities: activities);
        await Repository.CreateAsync(document);

        var existingResolution = ImportActivityResolution.Existing(activityId);
        var existingResult = await Repository.SetActivityResolutionAsync(document.Id, source.Id, existingResolution);
        var afterExisting = await Repository.FindByIdAsync(document.Id);
        var createNewResult = await Repository.SetActivityResolutionAsync(
            document.Id,
            source.Id,
            ImportActivityResolution.CreateNew);
        var afterCreateNew = await Repository.FindByIdAsync(document.Id);
        var resetResult = await Repository.SetActivityResolutionAsync(document.Id, source.Id, null);
        var afterReset = await Repository.FindByIdAsync(document.Id);

        Assert.True(existingResult.Success);
        Assert.True(afterExisting.Success);
        Assert.Equal(existingResolution, afterExisting.Value.Activities[0].Resolution);
        Assert.True(createNewResult.Success);
        Assert.True(afterCreateNew.Success);
        Assert.Equal(ImportActivityResolution.CreateNew, afterCreateNew.Value.Activities[0].Resolution);
        Assert.True(resetResult.Success);
        Assert.True(afterReset.Success);
        Assert.Null(afterReset.Value.Activities[0].Resolution);
    }

    [Fact]
    public async Task SetActivityResolutionAsync_ShouldFail_WhenActivityDoesNotExist()
    {
        var userId = await CreateUserAsync();
        var source = CreateSourceActivity("a1", "Coding");
        ImportDocumentActivity[] activities = [new(source)];
        var document = CreateDocument(userId, activities: activities);
        await Repository.CreateAsync(document);
        var missingId = ActivityId.Generate();
        var resolution = ImportActivityResolution.Existing(missingId);

        var result = await Repository.SetActivityResolutionAsync(document.Id, source.Id, resolution);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, result.Error.Type);
    }

    [Fact]
    public async Task SetActivityResolutionAsync_ShouldResetResolution_WhenExistingActivityIsRemoved()
    {
        var userId = await CreateUserAsync();
        var dbActivity = EntitiesFactory.CreateActivity();
        await Database.Activities.AddAsync(dbActivity);
        await Database.SaveChangesAsync();
        var activityId = ActivityId.Parse(dbActivity.Id, null);
        var source = CreateSourceActivity("a1", "Coding");
        var resolution = ImportActivityResolution.Existing(activityId);
        ImportDocumentActivity[] activities = [new(source, resolution)];
        var document = CreateDocument(userId, activities: activities);
        await Repository.CreateAsync(document);

        await Database.Activities.ExecuteDeleteAsync();

        var actual = await Repository.FindByIdAsync(document.Id);
        Assert.True(actual.Success);
        Assert.Null(actual.Value.Activities[0].Resolution);
    }

    [Fact]
    public async Task SetTagResolutionAsync_ShouldSaveExistingAndFailForUnknownSource()
    {
        var userId = await CreateUserAsync();
        var dbTag = EntitiesFactory.CreateTag();
        await Database.Tags.AddAsync(dbTag);
        await Database.SaveChangesAsync();
        var tagId = TagId.Parse(dbTag.Id, null);
        var source = CreateSourceTag("t1", "Work");
        ImportDocumentTag[] tags = [new(source)];
        var document = CreateDocument(userId, tags: tags);
        await Repository.CreateAsync(document);
        var resolution = ImportTagResolution.Existing(tagId);
        var unknownSourceId = ImportTagId.Parse("unknown", null);

        var result = await Repository.SetTagResolutionAsync(document.Id, source.Id, resolution);
        var unknownResult = await Repository.SetTagResolutionAsync(document.Id, unknownSourceId, resolution);

        Assert.True(result.Success);
        var actual = await Repository.FindByIdAsync(document.Id);
        Assert.True(actual.Success);
        Assert.Equal(resolution, actual.Value.Tags[0].Resolution);
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
