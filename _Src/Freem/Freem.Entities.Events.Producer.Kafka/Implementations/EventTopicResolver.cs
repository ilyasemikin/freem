using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Events.Producer.Kafka.Implementations;

public sealed class EventTopicResolver
{
    public string Resolve(IEntityEvent<IEntityIdentifier, UserIdentifier> @event)
    {
        return "some";
    }
}