using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Users.Models;
using Freem.Time;

namespace Freem.Domain.Entities.Records;

public sealed class RecordPeriod : IEquatable<RecordPeriod>
{
    public RecordPeriodId Id { get; }
    public DateTimePeriod Period { get; }
    public TimeZoneOffset TimeZoneOffset { get; }
    public RecordPeriodDescription? Description { get; }

    public RecordPeriod(
        RecordPeriodId id,
        DateTimePeriod period,
        TimeZoneOffset timeZoneOffset,
        RecordPeriodDescription? description = null)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(period);
        ArgumentNullException.ThrowIfNull(timeZoneOffset);

        if (period.Duration <= TimeSpan.Zero)
            throw new ArgumentException("Period of record must have positive duration", nameof(period));

        Id = id;
        Period = period;
        TimeZoneOffset = timeZoneOffset;
        Description = description;
    }

    public bool Equals(RecordPeriod? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return
            Id.Equals(other.Id) &&
            Period.Equals(other.Period) &&
            TimeZoneOffset.Equals(other.TimeZoneOffset) &&
            Equals(Description, other.Description);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is RecordPeriod other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Period, TimeZoneOffset, Description);
    }
}
