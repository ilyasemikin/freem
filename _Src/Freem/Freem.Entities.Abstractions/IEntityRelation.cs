using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Abstractions.Relations.Collection;

namespace Freem.Entities.Abstractions;

public interface IEntityRelation<TRelatedEntity, TRelatedEntityIdentifier>
    where TRelatedEntity : IEntity<TRelatedEntityIdentifier>
    where TRelatedEntityIdentifier : IEntityIdentifier
{
    IReadOnlyRelatedEntitiesCollection<TRelatedEntity, TRelatedEntityIdentifier> RelatedEntities { get; }
}