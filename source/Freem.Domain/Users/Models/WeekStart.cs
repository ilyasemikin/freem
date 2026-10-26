using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Users.Models;

public sealed class WeekStart : IEquatable<WeekStart>, IParsable<WeekStart>
{
    private const int DaysInWeek = 7;

    private readonly Value _value;

    public static WeekStart Monday { get; } = new(Value.Monday);
    public static WeekStart Sunday { get; } = new(Value.Sunday);

    public static WeekStart Default => Monday;

    public WeekStart(Value value)
    {
        if (!Enum.IsDefined(value))
            throw new ArgumentOutOfRangeException(nameof(value), value, null);

        _value = value;
    }

    public DayOfWeek DayOfWeek => _value switch
    {
        Value.Monday => DayOfWeek.Monday,
        Value.Sunday => DayOfWeek.Sunday,
        _ => throw new InvalidOperationException($"Unknown week start [{_value}]")
    };

    public DateOnly GetWeekStart(DateOnly date)
    {
        var diff = ((int)date.DayOfWeek - (int)DayOfWeek + DaysInWeek) % DaysInWeek;
        return date.AddDays(-diff);
    }

    public bool Equals(WeekStart? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is WeekStart other && Equals(other);
    }

    public override int GetHashCode()
    {
        return (int)_value;
    }

    public override string ToString()
    {
        return _value.ToString("G");
    }

    public Value ToValue()
    {
        return _value;
    }

    public static WeekStart Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for WeekStart: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out WeekStart? result)
    {
        if (!Enum.TryParse<Value>(s, out var value) || !Enum.IsDefined(value) || int.TryParse(s, out _))
        {
            result = null;
            return false;
        }

        result = new WeekStart(value);
        return true;
    }

    public enum Value
    {
        Monday,
        Sunday
    }
}
