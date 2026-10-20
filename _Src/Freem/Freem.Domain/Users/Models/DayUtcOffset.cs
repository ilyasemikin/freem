using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Freem.Domain.Users.Models;

public sealed class DayUtcOffset : IParsable<DayUtcOffset>
{
    private const string Format = "c";

    public static TimeSpan MinValue { get; } = TimeSpan.FromHours(-12);
    public static TimeSpan MaxValue { get; } = TimeSpan.FromHours(14);

    private readonly TimeSpan _value;
    
    public static DayUtcOffset Default { get; } = new(TimeSpan.Zero);
    
    public TimeSpan Value => _value;
    public long Ticks => _value.Ticks;
    
    public DayUtcOffset(TimeSpan value)
    {
        if (!IsValid(value))
            throw new ArgumentOutOfRangeException(nameof(value), value, $"Offset must be whole minutes in range [{MinValue}, {MaxValue}].");
        
        _value = value;
    }

    public DateTime GetDayStartUtc(DateOnly date)
    {
        var midnight = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        return midnight - _value;
    }

    public override string ToString()
    {
        return _value.ToString(Format, CultureInfo.InvariantCulture);
    }

    public static DayUtcOffset Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for DayUtcOffset: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out DayUtcOffset? result)
    {
        if (!TimeSpan.TryParseExact(s, Format, CultureInfo.InvariantCulture, out var value) || !IsValid(value))
        {
            result = null;
            return false;
        }

        result = new DayUtcOffset(value);
        return true;
    }

    private static bool IsValid(TimeSpan value)
    {
        return value >= MinValue && value <= MaxValue && value.Ticks % TimeSpan.TicksPerMinute == 0;
    }
}