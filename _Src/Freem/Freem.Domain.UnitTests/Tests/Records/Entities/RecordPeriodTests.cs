using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Time.Models;

namespace Freem.Domain.UnitTests.Tests.Records.Entities;

public sealed class RecordPeriodTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenDurationIsZero()
    {
        var now = new DateTimeOffset(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);
        var id = RecordPeriodId.Generate();
        var period = new DateTimePeriod(now, now);

        var exception = Xunit.Record.Exception(() => new RecordPeriod(id, period));

        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void Constructor_ShouldSuccess_WhenDurationIsPositive()
    {
        var now = new DateTimeOffset(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);
        var id = RecordPeriodId.Generate();
        var period = new DateTimePeriod(now, now.AddSeconds(1));

        var recordPeriod = new RecordPeriod(id, period);

        Assert.Equal(period, recordPeriod.Period);
        Assert.Null(recordPeriod.Description);
    }
}
