using System.Diagnostics.CodeAnalysis;

namespace Freem.Entities;

public class TimePeriod : IEquatable<TimePeriod>
{
    public DateTimeOffset StartAt { get; }
    public DateTimeOffset EndAt { get; }

    public TimeSpan Duration => EndAt - StartAt;

    public TimePeriod(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        StartAt = startAt.UtcDateTime;
        EndAt = endAt.UtcDateTime;

        if (StartAt > EndAt)
            throw new ArgumentException($"'{nameof(endAt)}' must be not less than '{nameof(endAt)}'");
    }

    public override string ToString()
    {
        return $"[{StartAt:O}, {EndAt:O}]";
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TimePeriod);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartAt, EndAt);
    }

    public bool Equals(TimePeriod? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return StartAt == other.StartAt && EndAt == other.EndAt;
    }

    public static bool TryCreate(DateTimeOffset startAt, DateTimeOffset endAt, [NotNullWhen(true)] out TimePeriod? period)
    {
        try
        {
            period = new TimePeriod(startAt, endAt);
        }
        catch (ArgumentException)
        {
            period = null;
            return false;
        }

        return true;
    }

    public static implicit operator TimeSpan(TimePeriod period)
    {
        return period.Duration;
    }

    public static bool operator ==(TimePeriod? left, TimePeriod? right)
    {
        if (left is null)
        {
            if (right is null)
                return true;

            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(TimePeriod? left, TimePeriod? right)
    {
        return !(left == right);
    }
}
