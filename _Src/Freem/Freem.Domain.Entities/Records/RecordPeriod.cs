using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Time.Models;

namespace Freem.Domain.Entities.Records;

public sealed class RecordPeriod : IEquatable<RecordPeriod>
{
    public RecordPeriodId Id { get; }
    public DateTimePeriod Period { get; }
    public RecordPeriodDescription? Description { get; }

    public RecordPeriod(
        RecordPeriodId id,
        DateTimePeriod period,
        RecordPeriodDescription? description = null)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(period);

        if (period.Duration <= TimeSpan.Zero)
            throw new ArgumentException("Period of record must have positive duration", nameof(period));

        Id = id;
        Period = period;
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
            Equals(Description, other.Description);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is RecordPeriod other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Period, Description);
    }
}
