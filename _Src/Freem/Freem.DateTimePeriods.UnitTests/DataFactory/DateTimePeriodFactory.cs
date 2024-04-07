using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Freem.DateTimePeriods.UnitTests.DataFactory;

internal static class DateTimePeriodFactory
{
    public static IEnumerable<TestCaseData> CreateTwoOverlapsedPeriods()
    {
        foreach (var data in CreateTwoOverlapsedPeriodsData())
        {
            yield return new TestCaseData(data.FirstPeriod, data.SecondPeriod);
            yield return new TestCaseData(data.SecondPeriod, data.FirstPeriod);
        }
    }

    public static IEnumerable<TestCaseData> CreateTwoNonOverlapsedPeriods()
    {
        foreach (var data in CreateTwoNonOverlapsedPeriodsData())
        {
            yield return new TestCaseData(data.FirstPeriod, data.SecondPeriod);
            yield return new TestCaseData(data.SecondPeriod, data.FirstPeriod);
        }
    }

    public static IEnumerable<TestCaseData> CreateTwoOverlapsedPeriodsWithCombined()
    {
        foreach (var data in CreateTwoOverlapsedPeriodsData())
        {
            yield return new TestCaseData(data.FirstPeriod, data.SecondPeriod, data.CombinedPeriod);
            yield return new TestCaseData(data.SecondPeriod, data.FirstPeriod, data.CombinedPeriod);
        }
    }

    public static IEnumerable<TestCaseData> CreateTwoOverlapsedPeriodsWithDuration()
    {
        foreach (var data in CreateTwoOverlapsedPeriodsData())
        {
            yield return new TestCaseData(data.FirstPeriod, data.SecondPeriod, data.CombinedPeriod.Duration);
            yield return new TestCaseData(data.SecondPeriod, data.FirstPeriod, data.CombinedPeriod.Duration);
        }
    }

    public static IEnumerable<TestCaseData> CreateTwoNonOverlapsedPeriodsWithDuration()
    {
        foreach (var data in CreateTwoNonOverlapsedPeriodsData())
        {
            yield return new TestCaseData(data.FirstPeriod, data.SecondPeriod, data.SummaryDuration);
            yield return new TestCaseData(data.SecondPeriod, data.FirstPeriod, data.SummaryDuration);
        }
    }

    public static IEnumerable<TestCaseData> CreateMultiplePeriods()
    {
        var startAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);

        {
            var periods = new DateTimePeriod[]
            {
                new (startAt.AddHours(12), startAt.AddHours(14)),
                new (startAt.AddHours(4), startAt.AddHours(6)),
                new (startAt, startAt.AddHours(2)),
                new (startAt.AddHours(8), startAt.AddHours(10))
            };

            var duration = TimeSpan.FromHours(8);

            yield return new TestCaseData(periods, duration);
        }

        {
            var period = new DateTimePeriod(startAt, startAt.AddHours(8));
            var periods = Enumerable.Range(0, 10)
                .Select(_ => period)
                .ToArray();

            var duration = TimeSpan.FromHours(8);

            yield return new TestCaseData(periods, duration);
        }

        {
            var endAt = startAt.AddHours(8);

            var periods = new DateTimePeriod[]
            {
                new (startAt, endAt),
                new (startAt.AddHours(1), endAt),
                new (startAt.AddHours(2), endAt),
                new (startAt.AddHours(3), endAt),
                new (startAt.AddHours(4), endAt)
            };

            var duration = TimeSpan.FromHours(8);

            yield return new TestCaseData(periods, duration);
        }

        {
            var periods = new DateTimePeriod[]
            {
                new (startAt, startAt.AddHours(8)),
                new (startAt, startAt.AddHours(7)),
                new (startAt, startAt.AddHours(6)),
                new (startAt, startAt.AddHours(5)),
                new (startAt, startAt.AddHours(4))
            };

            var duration = TimeSpan.FromHours(8);

            yield return new TestCaseData(periods, duration);
        }

        {
            var periods = new DateTimePeriod[]
            {
                new (startAt, startAt.AddHours(9)),
                new (startAt.AddHours(3), startAt.AddHours(12)),
                new (startAt.AddHours(2), startAt.AddHours(10))
            };

            var duration = TimeSpan.FromHours(12);

            yield return new TestCaseData(periods, duration);
        }

        {
            var periods = new DateTimePeriod[]
            {
                new (startAt, startAt.AddHours(9)),
                new (startAt.AddHours(3), startAt.AddHours(12)),
                new (startAt.AddHours(2), startAt.AddHours(10)),
                new (startAt.AddHours(14), startAt.AddHours(20))
            };

            var duration = TimeSpan.FromHours(18);

            yield return new TestCaseData(periods, duration);
        }
    }

    private static IEnumerable<TwoNonOverlapsedPeriodsData> CreateTwoNonOverlapsedPeriodsData()
    {
        var startAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);

        {
            var first = new DateTimePeriod(startAt, startAt.AddHours(4));
            var second = new DateTimePeriod(startAt.AddHours(8), startAt.AddHours(12));

            var duration = TimeSpan.FromHours(8);

            yield return new TwoNonOverlapsedPeriodsData(first, second, duration);
        }
    }

    private static IEnumerable<TwoOverlapsedPeriodsData> CreateTwoOverlapsedPeriodsData()
    {
        var startAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);

        {
            var first = new DateTimePeriod(startAt, startAt.AddHours(4));
            var second = new DateTimePeriod(startAt.AddHours(4), startAt.AddHours(8));

            var combined = new DateTimePeriod(startAt, startAt.AddHours(8));

            yield return new TwoOverlapsedPeriodsData(first, second, combined);
        }

        {
            var first = new DateTimePeriod(startAt, startAt.AddHours(4));
            var second = new DateTimePeriod(startAt.AddHours(2), startAt.AddHours(6));

            var combined = new DateTimePeriod(startAt, startAt.AddHours(6));

            yield return new TwoOverlapsedPeriodsData(first, second, combined);
        }

        {
            var first = new DateTimePeriod(startAt, startAt.AddHours(4));
            var second = new DateTimePeriod(startAt.AddHours(1), startAt.AddHours(3));

            var combined = new DateTimePeriod(startAt, startAt.AddHours(4));

            yield return new TwoOverlapsedPeriodsData(first, second, combined);
        }
    }

    private record TwoNonOverlapsedPeriodsData(
        DateTimePeriod FirstPeriod,
        DateTimePeriod SecondPeriod,
        TimeSpan SummaryDuration);

    private record TwoOverlapsedPeriodsData(
        DateTimePeriod FirstPeriod,
        DateTimePeriod SecondPeriod,
        DateTimePeriod CombinedPeriod);
}
