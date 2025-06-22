using System.Diagnostics.CodeAnalysis;
using Freem.Enums.Exceptions;

namespace Freem.Time.Models;

public enum Month
{
    January,
    February,
    March,
    April,
    May,
    June,
    July,
    August,
    September,
    October,
    November,
    December
}

public static class MonthFactory
{
    public static bool TryCreate(int value, [NotNullWhen(true)] out Month? result)
    {
        result = value switch
        {
            1 => Month.January,
            2 => Month.February,
            3 => Month.March,
            4 => Month.April,
            5 => Month.May,
            6 => Month.June,
            7 => Month.July,
            8 => Month.August,
            9 => Month.September,
            10 => Month.October,
            11 => Month.November,
            12 => Month.December,
            _ => null
        };
        
        return result is not null;
    }
}

public static class MonthExtensions
{
    public static int ToMonthNumber(this Month value)
    {
        return value switch
        {
            Month.January => 1,
            Month.February => 2,
            Month.March => 3,
            Month.April => 4,
            Month.May => 5,
            Month.June => 6,
            Month.July => 7,
            Month.August => 8,
            Month.September => 9,
            Month.October => 10,
            Month.November => 11,
            Month.December => 12,
            _ => throw new InvalidEnumValueException<Month>(value)
        };
    }
}