using Freem.Domain.Entities.Records;
using Freem.Domain.UseCases.Contracts.Filter;

namespace Freem.Domain.UseCases.Contracts.Records.ListByPeriod;

public sealed class ListRecordByPeriodResponse
{
    public IReadOnlyList<Record> Records { get; }
    public TotalCount TotalCount { get; }

    public ListRecordByPeriodResponse(IReadOnlyList<Record> records, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(records);
        ArgumentNullException.ThrowIfNull(totalCount);

        Records = records;
        TotalCount = totalCount;
    }
}