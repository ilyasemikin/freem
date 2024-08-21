using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Factories;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Abstractions.Identifiers.Factories;
using Freem.Entities.Identifiers;
using Freem.Time.Abstractions;

namespace Freem.Entities.Factories.Base;

public abstract class BaseEventEntityFactory<TEventEntity, TEntity, TIdentifier>
    : IEventEntityFactory<TEventEntity, EventIdentifier, UserIdentifier, TEntity, TIdentifier>
    where TEventEntity : IEventEntity<EventIdentifier, UserIdentifier>
    where TEntity : IEntity<TIdentifier>
    where TIdentifier : IEntityIdentifier
{
    protected delegate TEventEntity CreateDelegate(
        TEntity entity, 
        EventAction action, 
        EventIdentifier id,
        DateTimeOffset now);
    
    private readonly IEntityIdentifierFactory<EventIdentifier> _identifierFactory;
    private readonly ICurrentTimeGetter _currentTimeGetter;
    private readonly CreateDelegate _createDelegate;

    protected BaseEventEntityFactory(
        IEntityIdentifierFactory<EventIdentifier> identifierFactory, 
        ICurrentTimeGetter currentTimeGetter, 
        CreateDelegate createDelegate)
    {
        _identifierFactory = identifierFactory;
        _currentTimeGetter = currentTimeGetter;
        _createDelegate = createDelegate;
    }

    public TEventEntity Create(TEntity entity, EventAction action)
    {
        var id = _identifierFactory.Create();
        var now = _currentTimeGetter.Get();

        return _createDelegate(entity, action, id, now);
    }
}