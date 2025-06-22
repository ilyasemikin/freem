using System.Diagnostics.CodeAnalysis;
using Freem.Time.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Web.Api.Public.ModelBinders.Extensions;

internal static class ValueProviderResultExtensions
{
    public static bool TryGetStringValue(
        this ValueProviderResult result,
        string? defaultValue,
        [NotNullWhen(true)] out string? value)
    {
        value = defaultValue;
        if (result.Length == 0)
            return defaultValue is not null;

        if (result.Length != 1 || result.FirstValue is null)
            return false;

        value = result.FirstValue;
        return true;
    }

    public static bool TryGetIntValue(
        this ValueProviderResult result,
        int? defaultValue,
        [NotNullWhen(true)] out int? value)
    {
        value = defaultValue;
        if (result.Length == 0)
            return defaultValue.HasValue;

        if (result.Length != 1 || result.FirstValue is null)
            return false;

        if (!int.TryParse(result.FirstValue, out var actualValue))
            return false;

        value = actualValue;
        return true;
    }

    public static bool TryGetDateTimeOffsetValue(
        this ValueProviderResult result,
        DateTimeOffset? defaultValue,
        [NotNullWhen(true)] out DateTimeOffset? value)
    {
        value = defaultValue;
        if (result.Length == 0)
            return defaultValue.HasValue;

        if (result.Length != 1 || result.FirstValue is null)
            return false;

        if (!DateTimeOffset.TryParse(result.FirstValue, out var actualValue))
            return false;

        value = actualValue;
        return true;
    }

    public static bool TryGetDatePeriodValue(
        this ValueProviderResult result,
        DatePeriod? defaultValue,
        [NotNullWhen(true)] out DatePeriod? value)
    {
        value = defaultValue;
        if (result.Length == 0)
            return defaultValue is not null;

        if (result.Length != 1 || result.FirstValue is null)
            return false;

        if (!DatePeriod.TryParse(result.FirstValue, out var actualValue))
            return false;

        value = actualValue;
        return true;
    }

    public static bool TryGetDateTimePeriodValue(
        this ValueProviderResult result,
        DateTimePeriod? defaultValue,
        [NotNullWhen(true)] out DateTimePeriod? value)
    {
        value = defaultValue;
        if (result.Length == 0)
            return defaultValue is not null;

        if (result.Length != 1 || result.FirstValue is null)
            return false;

        if (!DateTimePeriod.TryParse(result.FirstValue, out var actualValue))
            return false;

        value = actualValue;
        return true;
    }

    public static bool TryGetEnumValue<TEnum>(
        this ValueProviderResult result,
        [NotNullWhen(true)] out TEnum? value)
        where TEnum : struct, Enum
    {
        value = null;
        if (result.Length == 0)
            return false;

        if (result.Length != 1 || result.FirstValue is null)
            return false;

        if (!Enum.TryParse<TEnum>(result.FirstValue, true, out var actualValue))
            return false;
        
        value = actualValue;
        return true;
    }

    public static bool TryGetMonthOnlyValue(
        this ValueProviderResult result,
        [NotNullWhen(true)] out MonthOnly? value)
    {
        value = null;
        if (result.Length == 0)
            return false;

        if (result.Length != 1 || result.FirstValue is null)
            return false;

        if (!MonthOnly.TryParse(result.FirstValue, out var actualValue))
            return false;
        
        value = actualValue;
        return true;
    }
    
    public static bool TryGetDateOnlyValue(
        this ValueProviderResult result,
        [NotNullWhen(true)] out DateOnly? value)
    {
        value = null;
        if (result.Length == 0)
            return false;

        if (result.Length != 1 || result.FirstValue is null)
            return false;

        if (!DateOnly.TryParse(result.FirstValue, out var actualValue))
            return false;
        
        value = actualValue;
        return true;
    }
}