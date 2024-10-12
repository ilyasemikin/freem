using Freem.Entities.Tags;
using Freem.Entities.UseCases.Models.Filter;

namespace Freem.Entities.UseCases.Tags.List.Models;

public sealed class ListTagResponse : IAsyncEnumerable<Tag>
{
    private readonly IAsyncEnumerable<Tag> _entities;
    
    public TotalCount TotalCount { get; }

    public ListTagResponse(IAsyncEnumerable<Tag> entities, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(entities);
        ArgumentNullException.ThrowIfNull(totalCount);

        _entities = entities;

        TotalCount = totalCount;
    }
    
    public IAsyncEnumerator<Tag> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return _entities.GetAsyncEnumerator(cancellationToken);
    }
}