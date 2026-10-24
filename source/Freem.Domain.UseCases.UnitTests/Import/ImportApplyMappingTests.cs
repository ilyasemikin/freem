using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Entities.Import;
using Freem.Domain.Import;
using Freem.Domain.Records.Models;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Domain.UseCases.Implementations.Import;
using Freem.Domain.Users;
using Freem.Domain.Users.Models;
using Freem.Time;

namespace Freem.Domain.UseCases.UnitTests.Import;

public sealed class ImportApplyMappingTests
{
    private static readonly DateTimeOffset Start = new(2026, 10, 1, 8, 0, 0, TimeSpan.Zero);
    private static readonly UserId UserId = UserId.Generate();

    private static ImportActivityId ActivityKey(string value)
    {
        return ImportActivityId.Parse(value, null);
    }

    private static ImportTagId TagKey(string value)
    {
        return ImportTagId.Parse(value, null);
    }

    private static ImportDocumentActivity CreateImportActivity(string key, ActivityId? mappedActivityId)
    {
        var name = ActivityName.Parse("Activity " + key, null);
        var activity = new ImportActivity(ActivityKey(key), name, null, ActivityStatus.Active, []);
        return new ImportDocumentActivity(activity, mappedActivityId);
    }

    private static ImportDocumentTag CreateImportTag(string key, TagId? mappedTagId)
    {
        var name = TagName.Parse("Tag " + key, null);
        var tag = new ImportTag(TagKey(key), name);
        return new ImportDocumentTag(tag, mappedTagId);
    }

    private static ImportRecordPeriod CreatePeriod(int startMinutes, int durationMinutes, TimeSpan offset)
    {
        var start = Start.AddMinutes(startMinutes);
        var end = start.AddMinutes(durationMinutes);
        var period = new DateTimePeriod(start, end);
        var timeZoneOffset = new TimeZoneOffset(offset);
        return new ImportRecordPeriod(period, timeZoneOffset);
    }

    private static ImportRow CreateRow(string[] activityKeys, string[] tagKeys, params ImportRecordPeriod[] periods)
    {
        var recordId = ImportRecordId.Parse("r1", null);
        var name = RecordName.Parse("Note", null);
        var activityIds = activityKeys.Select(ActivityKey).ToArray();
        var tagIds = tagKeys.Select(TagKey).ToArray();
        var record = new ImportRecord(recordId, name, null, activityIds, tagIds, periods);
        var rowId = ImportRowId.Generate();
        var documentId = ImportDocumentId.Generate();
        return new ImportRow(rowId, documentId, record);
    }

    private static ImportRow CreateRow(params string[] activityKeys)
    {
        var period = CreatePeriod(0, 10, TimeSpan.Zero);
        return CreateRow(activityKeys, [], period);
    }

    [Fact]
    public void FindUnmapped_ShouldReturnOnlyUnmappedEntitiesUsedByRows()
    {
        ImportDocumentActivity[] activities =
        [
            CreateImportActivity("a1", ActivityId.Generate()),
            CreateImportActivity("a2", null),
            CreateImportActivity("a3", null)
        ];
        ImportDocumentTag[] tags = [CreateImportTag("t1", null), CreateImportTag("t2", null)];
        var mappingCheck = new ImportMappingCheck(activities, tags);
        var period = CreatePeriod(0, 10, TimeSpan.Zero);

        mappingCheck.Add(CreateRow(["a1", "a2"], ["t1"], period));

        var actualActivities = mappingCheck.FindUnmappedActivities().Select(id => id.ToString()).ToArray();
        var actualTags = mappingCheck.FindUnmappedTags().Select(id => id.ToString()).ToArray();
        Assert.Equal(["a2"], actualActivities);
        Assert.Equal(["t1"], actualTags);
    }

    [Fact]
    public void FindUnmapped_ShouldReturnEntitiesAbsentInDocument()
    {
        var mappingCheck = new ImportMappingCheck([], []);

        mappingCheck.Add(CreateRow("missing"));

        var actual = mappingCheck.FindUnmappedActivities().Select(id => id.ToString()).ToArray();
        Assert.Equal(["missing"], actual);
    }

    [Fact]
    public void FindUnmapped_ShouldReturnNothing_WhenAllUsedEntitiesAreMapped()
    {
        ImportDocumentActivity[] activities = [CreateImportActivity("a1", ActivityId.Generate())];
        var mappingCheck = new ImportMappingCheck(activities, []);

        mappingCheck.Add(CreateRow("a1"));

        Assert.Empty(mappingCheck.FindUnmappedActivities());
        Assert.Empty(mappingCheck.FindUnmappedTags());
    }

    [Fact]
    public void TryCreate_ShouldUseMappingsAndKeepPeriodsWithOffsets()
    {
        var activityId = ActivityId.Generate();
        var tagId = TagId.Generate();
        ImportDocumentActivity[] activities =
        [
            CreateImportActivity("a1", activityId),
            CreateImportActivity("a2", activityId)
        ];
        ImportDocumentTag[] tags = [CreateImportTag("t1", tagId)];
        var mappings = new ImportMappings(activities, tags);
        var first = CreatePeriod(0, 10, TimeSpan.FromHours(3));
        var second = CreatePeriod(20, 10, TimeSpan.FromHours(-5));
        var row = CreateRow(["a1", "a2"], ["t1"], first, second);

        var record = ImportRecordFactory.TryCreate(row, UserId, mappings);

        Assert.NotNull(record);
        Assert.Equal(UserId, record.UserId);
        Assert.Equal(row.Record.Name, record.Name);
        Assert.Equal([activityId], record.Activities.Ids);
        Assert.Equal([tagId], record.Tags.Ids);
        Assert.Equal(2, record.Periods.Count);
        Assert.Equal(first.Period, record.Periods[0].Period);
        Assert.Equal(first.TimeZoneOffset, record.Periods[0].TimeZoneOffset);
        Assert.Equal(second.Period, record.Periods[1].Period);
        Assert.Equal(second.TimeZoneOffset, record.Periods[1].TimeZoneOffset);
    }

    [Fact]
    public void TryCreate_ShouldReturnNull_WhenPeriodHasZeroDuration()
    {
        var mappings = new ImportMappings([CreateImportActivity("a1", ActivityId.Generate())], []);
        var period = CreatePeriod(0, 0, TimeSpan.Zero);
        var row = CreateRow(["a1"], [], period);

        var record = ImportRecordFactory.TryCreate(row, UserId, mappings);

        Assert.Null(record);
    }

    [Fact]
    public void TryCreate_ShouldReturnNull_WhenPeriodsOverlap()
    {
        var mappings = new ImportMappings([CreateImportActivity("a1", ActivityId.Generate())], []);
        var first = CreatePeriod(0, 30, TimeSpan.Zero);
        var second = CreatePeriod(10, 30, TimeSpan.Zero);
        var row = CreateRow(["a1"], [], first, second);

        var record = ImportRecordFactory.TryCreate(row, UserId, mappings);

        Assert.Null(record);
    }

    [Fact]
    public void TryCreate_ShouldThrow_WhenEntityIsNotMapped()
    {
        var mappings = new ImportMappings([CreateImportActivity("a1", null)], []);
        var row = CreateRow("a1");

        var exception = Record.Exception(() => ImportRecordFactory.TryCreate(row, UserId, mappings));

        Assert.IsType<InvalidOperationException>(exception);
    }
}
