namespace Freem.Domain.Relations.Collections;

public interface IRelatedIdsCollection<TEntityId> 
    : IReadOnlyRelatedIdsCollection<TEntityId>
    where TEntityId : Id
{
    bool TryAdd(TEntityId id);
    bool TryRemove(TEntityId id);
}