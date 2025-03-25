using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters.Abstractions;

namespace Freem.Entities.Storage.Abstractions.Base.Search;

public interface IMultipleSearchByFilterRepository<TEntity, TEntityIdentifier, in TFilter>
    where TEntity : class, IEntity<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
    where TFilter : class, IFilter
{
    Task<SearchEntitiesAsyncResult<TEntity>> FindAsync(TFilter filter, CancellationToken cancellationToken = default);
}