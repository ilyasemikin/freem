using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Entities.DataTransfer.Export;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Domain.Users.Models;
using Freem.Time;

namespace Freem.Domain.UseCases.UnitTests.Fixtures;

internal static class ExportTestData
{
    private static readonly DateTimeOffset Base = new(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);

    public static ExportItem[] CreateItems()
    {
        var usedTag = new ExportTag(TagId.Generate(), TagName.Parse("deep work", null));
        var unusedTag = new ExportTag(TagId.Generate(), TagName.Parse("unused", null));

        var activeTags = new[] { usedTag.Id };
        var active = new ExportActivity(
            ActivityId.Generate(),
            ActivityName.Parse("coding", null),
            ActivityColor.Parse("#aa00ff", null),
            ActivityStatus.Active,
            activeTags);
        var archived = new ExportActivity(
            ActivityId.Generate(),
            ActivityName.Parse("reading", null),
            ActivityColor.Parse("#00ff00", null),
            ActivityStatus.Archived,
            []);

        var firstPeriod = CreatePeriod(0, 60, "morning");
        var secondPeriod = CreatePeriod(180, 240, null);
        var full = new ExportRecord(
            RecordId.Generate(),
            RecordName.Parse("feature", null),
            RecordDescription.Parse("implemented export", null),
            [active.Id, archived.Id],
            [usedTag.Id],
            [firstPeriod, secondPeriod]);
        var minimal = new ExportRecord(
            RecordId.Generate(),
            null,
            null,
            [archived.Id],
            [],
            [CreatePeriod(300, 330, null)]);

        return [usedTag, unusedTag, active, archived, full, minimal];
    }

    private static RecordPeriodSnapshot CreatePeriod(int startMinutes, int endMinutes, string? description)
    {
        var period = new DateTimePeriod(Base.AddMinutes(startMinutes), Base.AddMinutes(endMinutes));
        var offset = new TimeZoneOffset(TimeSpan.FromHours(3));
        var periodDescription = description is null
            ? null
            : RecordPeriodDescription.Parse(description, null);
        return new RecordPeriodSnapshot(RecordPeriodId.Generate(), period, offset, periodDescription);
    }
}
