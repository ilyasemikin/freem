using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Storage.Abstractions.Models;

namespace Freem.Entities.Storage.Abstractions.Base;

public interface ISearchByIdRepository<TEntity, in TEntityIdentifier>
    where TEntity : IEntity<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    Task<SearchEntityResult<TEntity>> FindByIdAsync(
        TEntityIdentifier id, 
        CancellationToken cancellationToken = default);
}
