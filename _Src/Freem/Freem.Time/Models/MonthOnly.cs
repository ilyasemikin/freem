using System.Diagnostics.CodeAnalysis;

namespace Freem.Time.Models;

public sealed class MonthOnly
{
    public Month Month { get; }
    public int Year { get; }

    public MonthOnly(Month month, int year)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(year);

        Month = month;
        Year = year;
    }

    public DateOnly ToDateOnly(int day)
    {
        return new DateOnly(Year, Month.ToMonthNumber(), day);
    }

    public override string ToString()
    {
        var month = Month.ToMonthNumber()
            .ToString()
            .PadLeft(2, '0');

        return $"{month}.{Year}";
    }

    public static bool TryParse(string value, [NotNullWhen(true)] out MonthOnly? result)
    {
        result = null;
        
        var parts = value.Split('.');
        if (parts.Length != 2)
            return false;

        if (!int.TryParse(parts[0], out var monthInt) ||
            !MonthFactory.TryCreate(monthInt, out var month) ||
            !int.TryParse(parts[1], out var year))
            return false;

        result = new MonthOnly(month.Value, year);
        return true;
    }
}