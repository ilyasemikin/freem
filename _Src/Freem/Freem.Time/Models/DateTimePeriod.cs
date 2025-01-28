using System.Diagnostics.CodeAnalysis;
using Freem.Time.Models.Helpers;

namespace Freem.Time.Models;

public sealed class DateTimePeriod : IEquatable<DateTimePeriod>
{
    public static DateTimePeriod Empty { get; } = new(DateTimeOffset.MinValue, DateTimeOffset.MinValue);

    public DateTimeOffset StartAt { get; }
    public DateTimeOffset EndAt { get; }

    public TimeSpan Duration => EndAt - StartAt;

    public DateTimePeriod(DateTimeOffset startAt, DateTimeOffset endAt)
    {
        StartAt = startAt.UtcDateTime;
        EndAt = endAt.UtcDateTime;

        if (StartAt > EndAt)
            throw new ArgumentException($"'{nameof(endAt)}' must be not less than '{nameof(startAt)}'");
    }

    public DateTimePeriod(DateTimeOffset startAt, TimeSpan duration)
        : this(startAt, startAt + duration)
    {
    }

    public override string ToString()
    {
        return $"[{StartAt:O}, {EndAt:O}]";
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as DateTimePeriod);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartAt, EndAt);
    }

    public bool Equals(DateTimePeriod? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return StartAt == other.StartAt && EndAt == other.EndAt;
    }

    public bool EqualsUpToSeconds(DateTimePeriod? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;
        
        return
            DateTimeOperations.EqualsUpToSeconds(StartAt, other.StartAt) &&
            DateTimeOperations.EqualsUpToSeconds(EndAt, other.EndAt);
    }

    public static bool TryCreate(DateTimeOffset startAt, DateTimeOffset endAt, [NotNullWhen(true)] out DateTimePeriod? period)
    {
        try
        {
            period = new DateTimePeriod(startAt, endAt);
        }
        catch (ArgumentException)
        {
            period = null;
            return false;
        }

        return true;
    }

    public static bool IsOverlapsed(DateTimePeriod left, DateTimePeriod right)
    {
        return !IsNonOverlapsed(left, right);
    }

    public static bool IsNonOverlapsed(DateTimePeriod left, DateTimePeriod right)
    {
        return left.EndAt < right.StartAt || right.EndAt < left.StartAt;
    }

    public static bool TryCombine(DateTimePeriod left, DateTimePeriod right, [NotNullWhen(true)] out DateTimePeriod? result)
    {
        if (IsNonOverlapsed(left, right))
        {
            result = null;
            return false;
        }

        var startAt = DateTimeOffsetComparer.Min(left.StartAt, right.StartAt);
        var endAt = DateTimeOffsetComparer.Max(left.EndAt, right.EndAt);

        result = new DateTimePeriod(startAt, endAt);
        return true;
    }

    public static TimeSpan CombineDuration(DateTimePeriod left, DateTimePeriod right)
    {
        if (IsNonOverlapsed(left, right))
            return left.Duration + right.Duration;

        var startAt = DateTimeOffsetComparer.Min(left.StartAt, right.StartAt);
        var endAt = DateTimeOffsetComparer.Max(left.EndAt, right.EndAt);

        return endAt - startAt;
    }

    public static implicit operator TimeSpan(DateTimePeriod period)
    {
        return period.Duration;
    }

    public static bool operator ==(DateTimePeriod? left, DateTimePeriod? right)
    {
        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(DateTimePeriod? left, DateTimePeriod? right)
    {
        return !(left == right);
    }
}
