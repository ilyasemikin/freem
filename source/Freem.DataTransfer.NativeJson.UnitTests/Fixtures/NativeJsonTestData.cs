using Freem.DataTransfer.NativeJson.Models;

namespace Freem.DataTransfer.NativeJson.UnitTests.Fixtures;

internal static class NativeJsonTestData
{
    private static readonly DateTimeOffset Base = new(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);

    public static NativeJsonItem[] CreateItems()
    {
        var usedTag = new NativeJsonTag { Id = "t1", Name = "deep work" };
        var unusedTag = new NativeJsonTag { Id = "t2", Name = "unused" };

        var active = new NativeJsonActivity
        {
            Id = "a1",
            Name = "coding",
            Color = "#aa00ff",
            Status = "Active",
            TagIds = ["t1"]
        };
        var archived = new NativeJsonActivity
        {
            Id = "a2",
            Name = "reading",
            Color = "#00ff00",
            Status = "Archived",
            TagIds = []
        };

        var firstPeriod = CreatePeriod("p1", 0, 60, "morning");
        var secondPeriod = CreatePeriod("p2", 180, 240, null);
        var full = new NativeJsonRecord
        {
            Id = "r1",
            Name = "feature",
            Description = "implemented export",
            ActivityIds = ["a1", "a2"],
            TagIds = ["t1"],
            Periods = [firstPeriod, secondPeriod]
        };
        var minimalPeriod = CreatePeriod("p3", 300, 330, null);
        var minimal = new NativeJsonRecord
        {
            Id = "r2",
            ActivityIds = ["a2"],
            TagIds = [],
            Periods = [minimalPeriod]
        };

        return [usedTag, unusedTag, active, archived, full, minimal];
    }

    private static NativeJsonPeriod CreatePeriod(string id, int startMinutes, int endMinutes, string? description)
    {
        return new NativeJsonPeriod
        {
            Id = id,
            StartAt = Base.AddMinutes(startMinutes),
            EndAt = Base.AddMinutes(endMinutes),
            TimeZoneOffset = "03:00:00",
            Description = description
        };
    }
}
