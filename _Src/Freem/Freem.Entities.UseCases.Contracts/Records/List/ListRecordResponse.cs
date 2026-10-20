using Freem.Domain.Entities.Records;
using Freem.Entities.UseCases.Contracts.Filter;

namespace Freem.Entities.UseCases.Contracts.Records.List;

public sealed class ListRecordResponse
{
    public IReadOnlyList<Record> Records { get; }
    public TotalCount TotalCount { get; }

    public ListRecordResponse(IReadOnlyList<Record> records, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(records);
        ArgumentNullException.ThrowIfNull(totalCount);

        Records = records;
        TotalCount = totalCount;
    }
}