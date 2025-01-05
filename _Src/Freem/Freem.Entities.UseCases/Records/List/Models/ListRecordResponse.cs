using System.Collections;
using Freem.Entities.Records;
using Freem.Entities.UseCases.Models.Filter;

namespace Freem.Entities.UseCases.Records.List.Models;

public sealed class ListRecordResponse : IEnumerable<Record>
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

    public ListRecordResponse(IEnumerable<Record> records, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(records);
        ArgumentNullException.ThrowIfNull(totalCount);
        
        Records = records.ToArray();
        TotalCount = totalCount;
    }

    public IEnumerator<Record> GetEnumerator()
    {
        return Records.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}