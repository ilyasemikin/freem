using System.Diagnostics.CodeAnalysis;

namespace Freem.Time.Models;

public sealed class DatePeriod : IEquatable<DatePeriod>
{
    public static DatePeriod Empty { get; } = new(DateOnly.MinValue, DateOnly.MinValue);
    
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

    public override string ToString()
    {
        return $"{StartAt:O},{EndAt:O}";
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is DatePeriod other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartAt, EndAt);
    }

    public static bool TryParse(string input, [NotNullWhen(true)] out DatePeriod? period)
    {
        period = null;
        
        var parts = input.Split(',');
        if (parts.Length != 2)
            return false;

        if (!DateOnly.TryParse(parts[0], out var startAt) || !DateOnly.TryParse(parts[1], out var endAt))
            return false;
        
        period = new DatePeriod(startAt, endAt);
        return true;
    }
}