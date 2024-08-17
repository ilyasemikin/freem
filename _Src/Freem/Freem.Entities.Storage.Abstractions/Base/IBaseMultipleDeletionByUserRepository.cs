using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Base;

public interface IBaseMultipleDeletionByUserRepository<TEntity, TEntityIdentifier>
    where TEntity : notnull, IEntity<TEntityIdentifier>
    where TEntityIdentifier : notnull, IEntityIdentifier
{
    Task<int> RemoveMultipleByUserAsync(
        UserIdentifier userId, 
        IEnumerable<TEntityIdentifier> ids, 
        CancellationToken cancellationToken = default);
}
