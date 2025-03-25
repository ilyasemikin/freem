using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Storage.Abstractions.Models;

namespace Freem.Entities.Storage.Abstractions.Base.Search;

public interface ISearchByMultipleIdsRepository<TEntity, TEntityIdentifier, in TMultipleEntityIdentifier>
    where TEntity : IEntity<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
    where TMultipleEntityIdentifier : class, IMultipleEntityIdentifier
{
    public Task<SearchEntityResult<TEntity>> FindByMultipleIdAsync(TMultipleEntityIdentifier ids, CancellationToken cancellationToken = default);
}