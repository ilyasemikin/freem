using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.UseCases.Contracts.Records.Create;
using Freem.Time.Models;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.Records;

internal static class RecordPeriodsFactory
{
    public static Error? ValidateDuration(DateTimePeriod period)
    {
        if (period.Duration <= TimeSpan.Zero)
            return DomainErrorCatalog.Factories.CreatePeriodIsInvalid("Period must have positive duration.");

        return null;
    }

    public static Result<RecordPeriodsCollection, Error> Create(IReadOnlyList<NewRecordPeriod> items)
    {
        if (items.Count < RecordPeriodsCollection.MinCount)
            return DomainErrorCatalog.Factories.CreatePeriodIsInvalid("Record must contain at least one period.");

        var periods = new List<RecordPeriod>();
        foreach (var item in items)
        {
            var error = ValidateDuration(item.Period);
            if (error is not null)
                return error;

            var id = RecordPeriodId.Generate();
            var period = new RecordPeriod(id, item.Period, item.Description);
            periods.Add(period);
        }

        if (!RecordPeriodsCollection.TryCreate(periods, out var collection))
            return DomainErrorCatalog.Factories.CreatePeriodIsInvalid("Periods of record must not overlap.");

        return collection;
    }
}
