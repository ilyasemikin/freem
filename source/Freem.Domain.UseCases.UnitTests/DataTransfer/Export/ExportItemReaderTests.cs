using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Entities.Activities;
using Freem.Domain.Entities.Records;
using RecordEntity = Freem.Domain.Entities.Records.Record;
using Freem.Domain.Entities.Tags;
using Freem.Domain.Entities.DataTransfer.Export;
using Freem.Domain.Records;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Domain.UseCases.Implementations.DataTransfer.Export;
using Freem.Domain.UseCases.UnitTests.Fixtures;
using Freem.Domain.Users;
using Freem.Domain.Users.Models;
using Freem.Time;
using Freem.Domain.Records.Sources;

namespace Freem.Domain.UseCases.UnitTests.DataTransfer.Export;

public sealed class ExportItemReaderTests
{
    private static readonly DateTimeOffset Base = new(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);

    private readonly UserId _userId = UserId.Generate();

    [Fact]
    public async Task ReadAsync_ShouldReadAllItemsAcrossBatches_InOrderTagsActivitiesRecords()
    {
        var tags = Enumerable.Range(0, 5).Select(i => CreateTag($"tag {i}")).ToArray();
        var activities = Enumerable.Range(0, 5).Select(i => CreateActivity($"activity {i}", tags[0].Id)).ToArray();
        var records = Enumerable.Range(0, 5).Select(i => CreateRecord(activities[0].Id, i)).ToArray();
        var reader = CreateReader(tags, activities, records, 2);

        var items = await ReadAllAsync(reader);

        Assert.Equal(15, items.Length);
        Assert.All(items.Take(5), item => Assert.IsType<ExportTag>(item));
        Assert.All(items.Skip(5).Take(5), item => Assert.IsType<ExportActivity>(item));
        Assert.All(items.Skip(10), item => Assert.IsType<ExportRecord>(item));
        Assert.Equal(tags.Select(t => t.Id), items.OfType<ExportTag>().Select(t => t.Id));
        Assert.Equal(activities.Select(a => a.Id), items.OfType<ExportActivity>().Select(a => a.Id));
        Assert.Equal(records.Select(r => r.Id), items.OfType<ExportRecord>().Select(r => r.Id));
    }

    [Fact]
    public async Task ReadAsync_ShouldReadAllItems_WhenCountIsMultipleOfBatchSize()
    {
        var tags = Enumerable.Range(0, 4).Select(i => CreateTag($"tag {i}")).ToArray();
        var reader = CreateReader(tags, [], [], 2);

        var items = await ReadAllAsync(reader);

        Assert.Equal(4, items.Length);
    }

    [Fact]
    public async Task ReadAsync_ShouldReturnNothing_WhenUserHasNoData()
    {
        var reader = CreateReader([], [], [], 2);

        var items = await ReadAllAsync(reader);

        Assert.Empty(items);
    }

    [Fact]
    public async Task ReadAsync_ShouldThrowExportReadException_WhenRepositoryFails()
    {
        var error = DomainErrorCatalog.Factories.CreateInternalServerError();
        var reader = new ExportItemReader(
            new FakeTagRepository([]),
            new FakeActivityRepository([]),
            new FakeRecordRepository([], error));

        var exception = await Xunit.Record.ExceptionAsync(() => ReadAllAsync(reader));

        var readException = Assert.IsType<ExportReadException>(exception);
        Assert.Equal(error.Type, readException.Error.Type);
    }

    [Fact]
    public void MapRecord_ShouldKeepMultiplePeriodsAndRelations()
    {
        var tag = CreateTag("tag");
        var activity = CreateActivity("activity", tag.Id);
        var record = CreateRecord(activity.Id, 0, tag.Id);
        record.Periods.TryAdd(CreatePeriod(120, 180));

        var item = ExportItemReader.MapRecord(record);

        Assert.Equal(record.Id, item.Id);
        Assert.Equal(2, item.Periods.Count);
        Assert.Equal([activity.Id], item.ActivityIds);
        Assert.Equal([tag.Id], item.TagIds);
    }

    [Fact]
    public void MapActivity_ShouldKeepArchivedStatusAndTags()
    {
        var tag = CreateTag("tag");
        var activity = CreateActivity("activity", tag.Id);
        activity.Status = ActivityStatus.Archived;

        var item = ExportItemReader.MapActivity(activity);

        Assert.Equal(ActivityStatus.Archived, item.Status);
        Assert.Equal([tag.Id], item.TagIds);
    }

    private ExportItemReader CreateReader(
        IReadOnlyList<Tag> tags,
        IReadOnlyList<Activity> activities,
        IReadOnlyList<RecordEntity> records,
        int batchSize)
    {
        return new ExportItemReader(
            new FakeTagRepository(tags),
            new FakeActivityRepository(activities),
            new FakeRecordRepository(records),
            batchSize);
    }

    private async Task<ExportItem[]> ReadAllAsync(ExportItemReader reader)
    {
        var result = new List<ExportItem>();
        await foreach (var item in reader.ReadAsync(_userId))
            result.Add(item);

        return result.ToArray();
    }

    private Tag CreateTag(string name)
    {
        var tagName = TagName.Parse(name, null);
        return new Tag(TagId.Generate(), _userId, tagName);
    }

    private Activity CreateActivity(string name, TagId tagId)
    {
        var activityName = ActivityName.Parse(name, null);
        var color = ActivityColor.Parse("#112233", null);
        var tags = new RelatedTagIdsCollection([tagId]);
        return new Activity(ActivityId.Generate(), _userId, activityName, color, tags, ActivityStatus.Active);
    }

    private RecordEntity CreateRecord(ActivityId activityId, int index, TagId? tagId = null)
    {
        var activities = new RelatedActivityIdsCollection([activityId]);
        var tags = tagId is null
            ? new RelatedTagIdsCollection()
            : new RelatedTagIdsCollection([tagId]);
        var periods = new RecordPeriodsCollection(CreatePeriod(index * 10, index * 10 + 5));
        return new RecordEntity(RecordId.Generate(), _userId, activities, tags, periods, ManualRecordSource.Instance);
    }

    private static RecordPeriod CreatePeriod(int startMinutes, int endMinutes)
    {
        var period = new DateTimePeriod(Base.AddMinutes(startMinutes), Base.AddMinutes(endMinutes));
        return new RecordPeriod(RecordPeriodId.Generate(), period, TimeZoneOffset.Default);
    }
}
