namespace Freem.Time.Models;

public sealed class DatePeriod : IEquatable<DatePeriod>
{
    public DateOnly StartAt { get; }
    public DateOnly EndAt { get; }

    public DatePeriod(DateOnly startAt, DateOnly endAt)
    {
        StartAt = startAt;
        EndAt = endAt;

        if (StartAt > EndAt)
            throw new ArgumentException($"'{nameof(endAt)}' must be not less than '{nameof(startAt)}'.");
    }

    public bool Equals(DatePeriod? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return 
            StartAt.Equals(other.StartAt) && 
            EndAt.Equals(other.EndAt);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is DatePeriod other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartAt, EndAt);
    }
}