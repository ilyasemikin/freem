using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters.Base;

namespace Freem.Entities.Storage.Abstractions.Base;

public interface IBaseSearchByFilterRepository<TEntity, TFilter, TSortField>
    where TFilter : BaseFilter<TSortField>
    where TSortField : Enum
{
    Task<SearchEntitiesAsyncResult<TEntity>> FindAsync(TFilter filter, CancellationToken cancellationToken = default);
}
