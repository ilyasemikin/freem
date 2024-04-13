using Freem.DateTimePeriods;

namespace Freem.Entities.Extensions;

public static class RunningRecordExtensions
{
    public static Record ToRecord(this RunningRecord running, string id, DateTimeOffset endAt)
    {
        var period = new DateTimePeriod(running.StartAt, endAt);
        return new Record(id, running.UserId, running.CategoryIds, running.TagIds, period);
    }
}
