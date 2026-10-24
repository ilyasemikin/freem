using Freem.Domain.Records.Models;
using Freem.Time;

namespace Freem.Domain.UseCases.Contracts.Records.Create;

public sealed class NewRecordPeriod
{
    public DateTimePeriod Period { get; }
    public RecordPeriodDescription? Description { get; init; }

    public NewRecordPeriod(DateTimePeriod period)
    {
        ArgumentNullException.ThrowIfNull(period);

        Period = period;
    }
}
