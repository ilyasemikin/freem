
namespace Freem.Entities.Storage.Abstractions.Models;

public class SearchEntitiesAsyncResult<TEntity> : IAsyncEnumerable<TEntity>
{
    private readonly IAsyncEnumerable<TEntity> _entities;

    public int TotalCount { get; }

    public SearchEntitiesAsyncResult(IAsyncEnumerable<TEntity> entities, int totalCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(totalCount);

        _entities = entities;

        TotalCount = totalCount;
    }

    public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return _entities.GetAsyncEnumerator(cancellationToken);
    }
}
