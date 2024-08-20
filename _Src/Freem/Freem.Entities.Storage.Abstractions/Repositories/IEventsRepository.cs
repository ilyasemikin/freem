using Freem.Entities.Abstractions;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Base;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IEventsRepository : 
    IBaseSearchByIdRepository<IEventEntity<EventIdentifier, UserIdentifier>, EventIdentifier>
{
}