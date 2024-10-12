using Freem.Entities.Activities;
using Freem.Entities.UseCases.Models.Filter;

namespace Freem.Entities.UseCases.Activities.List.Models;

public sealed class ListActivityResponse : IAsyncEnumerable<Activity>
{
    private readonly IAsyncEnumerable<Activity> _entities;

    public TotalCount TotalCount { get; }
    
    public ListActivityResponse(IAsyncEnumerable<Activity> entities, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(entities);
        ArgumentNullException.ThrowIfNull(totalCount);
        
        _entities = entities;
        
        TotalCount = totalCount;
    }
    
    public IAsyncEnumerator<Activity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return _entities.GetAsyncEnumerator(cancellationToken);
    }
}