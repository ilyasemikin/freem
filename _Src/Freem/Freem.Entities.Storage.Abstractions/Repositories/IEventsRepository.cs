using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Storage.Abstractions.Base.Write;
using Freem.Entities.Storage.Abstractions.Models.Filters;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IEventsRepository : 
    ICreateRepository<IEntityEvent<IEntityIdentifier, UserIdentifier>, EventIdentifier>,
    ISearchByIdRepository<IEntityEvent<IEntityIdentifier, UserIdentifier>, EventIdentifier>,
    IMultipleSearchByFilterRepository<IEntityEvent<IEntityIdentifier, UserIdentifier>, EventIdentifier, EventsAfterTimeFilter>
{
}