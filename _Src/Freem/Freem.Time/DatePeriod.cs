using System.Diagnostics.CodeAnalysis;

namespace Freem.Time;

public sealed class DatePeriod : IEquatable<DatePeriod>, IParsable<DatePeriod>
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

    public static DatePeriod Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for DatePeriod: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out DatePeriod? result)
    {
        result = null;
        if (s is null)
            return false;
        
        var parts = s.Split(',');
        if (parts.Length != 2)
            return false;

        if (!DateOnly.TryParse(parts[0], provider, out var startAt) || !DateOnly.TryParse(parts[1], provider, out var endAt))
            return false;

        if (startAt > endAt)
            return false;
        
        result = new DatePeriod(startAt, endAt);
        return true;
    }
}