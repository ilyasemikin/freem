using Freem.Entities.Records;
using Freem.Entities.UseCases.Models.Filter;

namespace Freem.Entities.UseCases.Records.List.Models;

public sealed class ListRecordResponse : IAsyncEnumerable<Record>
{
    private readonly IAsyncEnumerable<Record> _entities;
    
    public TotalCount TotalCount { get; }

    public ListRecordResponse(IAsyncEnumerable<Record> entities, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(entities);
        ArgumentNullException.ThrowIfNull(totalCount);

        _entities = entities;
        
        TotalCount = totalCount;
    }
    
    public IAsyncEnumerator<Record> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return _entities.GetAsyncEnumerator(cancellationToken);
    }
}