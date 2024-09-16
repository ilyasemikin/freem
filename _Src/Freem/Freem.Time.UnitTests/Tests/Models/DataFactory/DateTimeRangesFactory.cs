using NUnit.Framework;

namespace Freem.Time.UnitTests.Tests.Models.DataFactory;

internal static class DateTimeRangesFactory
{
    public static IEnumerable<TestCaseData> CreateCorrectDateTimeRange()
    {
        {
            var startAt = new DateTimeOffset(2024, 4, 1, 0, 0, 0, TimeSpan.Zero);
            var endAt = startAt.AddDays(1);
            yield return new TestCaseData(startAt, endAt);
        }

        {
            var startAt = new DateTimeOffset(2024, 4, 1, 0, 0, 0, TimeSpan.FromHours(3));
            var endAt = startAt.AddDays(1);
            yield return new TestCaseData(startAt, endAt);
        }

        {
            var startAt = new DateTimeOffset(2024, 4, 1, 0, 0, 0, TimeSpan.FromHours(3));
            var endAt = new DateTimeOffset(2024, 4, 1, 5, 0, 0, TimeSpan.FromHours(4));
            yield return new TestCaseData(startAt, endAt);
        }

        {
            var startAt = new DateTimeOffset(2024, 4, 1, 0, 0, 0, TimeSpan.FromHours(3));
            var endAt = new DateTimeOffset(2024, 4, 1, 1, 0, 0, TimeSpan.FromHours(4));
            yield return new TestCaseData(startAt, endAt);
        }

        {
            var startAt = new DateTimeOffset(2024, 4, 1, 0, 0, 0, TimeSpan.FromHours(3));
            var endAt = startAt;
            yield return new TestCaseData(startAt, endAt);
        }
    }

    public static IEnumerable<TestCaseData> CreateInvalidDateTimeRange()
    {
        {
            var startAt = new DateTimeOffset(2024, 4, 1, 0, 0, 0, TimeSpan.Zero);
            var endAt = new DateTimeOffset(2024, 3, 31, 23, 59, 59, TimeSpan.Zero);
            yield return new TestCaseData(startAt, endAt);
        }

        {
            var startAt = new DateTimeOffset(2024, 4, 1, 0, 0, 0, TimeSpan.FromHours(3));
            var endAt = new DateTimeOffset(2024, 4, 1, 0, 0, 0, TimeSpan.FromHours(4));
            yield return new TestCaseData(startAt, endAt);
        }
    }
}
