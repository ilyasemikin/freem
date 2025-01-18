using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Bus.Events.Abstractions;
using Freem.Identifiers.Abstractions.Generators;

namespace Freem.Entities.Bus.Events.Implementations;

internal class EventProducer : IEventProducer
{
    private readonly IIdentifierGenerator<EventIdentifier> _identifierGenerator;
    private readonly IEventPublisher _eventPublisher;

    public EventProducer(IIdentifierGenerator<EventIdentifier> identifierGenerator, IEventPublisher eventPublisher)
    {
        ArgumentNullException.ThrowIfNull(identifierGenerator);
        ArgumentNullException.ThrowIfNull(eventPublisher);
        
        _identifierGenerator = identifierGenerator;
        _eventPublisher = eventPublisher;
    }

    public async Task PublishAsync(IEventProducer.EventFactory factory, CancellationToken cancellationToken = default)
    {
        var eventId = _identifierGenerator.Generate();
        var @event = factory(eventId);
        
        await _eventPublisher.PublishAsync(@event, cancellationToken);
    }
}