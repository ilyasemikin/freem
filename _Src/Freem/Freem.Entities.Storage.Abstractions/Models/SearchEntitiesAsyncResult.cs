
namespace Freem.Entities.Storage.Abstractions.Models;

public class SearchEntitiesAsyncResult<TEntity> : IAsyncEnumerable<TEntity>
{
    private readonly IAsyncEnumerable<TEntity> _entities;

    public int TotalCount { get; }

    public SearchEntitiesAsyncResult(IAsyncEnumerable<TEntity> entities, int totalCount)
    {
        ArgumentNullException.ThrowIfNull(entities);
        ArgumentOutOfRangeException.ThrowIfNegative(totalCount);

        _entities = entities;

        TotalCount = totalCount;
    }

    public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return _entities.GetAsyncEnumerator(cancellationToken);
    }

    public static SearchEntitiesAsyncResult<TEntity> Create<TDatabaseEntity>(
        IAsyncEnumerable<TDatabaseEntity> enumerable, 
        Func<TDatabaseEntity, TEntity> mapper,
        int totalCount)
    {
        async IAsyncEnumerable<TEntity> CreateEntitiesEnumerable()
        {
            await foreach (var entity in enumerable)
                yield return mapper(entity);
        }

        var entities = CreateEntitiesEnumerable();
        return new SearchEntitiesAsyncResult<TEntity>(entities, totalCount);
    }
}
