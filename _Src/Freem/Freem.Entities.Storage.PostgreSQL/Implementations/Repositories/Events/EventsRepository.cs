using Freem.Entities.Abstractions;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events;

internal class EventsRepository : IEventsRepository
{
    private readonly DatabaseContext _context;

    public EventsRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<SearchEntityResult<IEventEntity<EventIdentifier, UserIdentifier>>> FindByIdAsync(
        EventIdentifier id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var dbEntity = await _context.Events.FirstOrDefaultAsync(e => e.Id == id.Value, cancellationToken);
        
        if (dbEntity is null)
            return SearchEntityResult<IEventEntity<EventIdentifier, UserIdentifier>>.NotFound();

        var entity = dbEntity.MapToDomainEntity();
        return SearchEntityResult<IEventEntity<EventIdentifier, UserIdentifier>>.Found(entity);
    }

    public async Task<SearchEntitiesAsyncResult<IEventEntity<EventIdentifier, UserIdentifier>>> FindAfterAsync(
        EventsAfterTimeFilter filter, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        return await _context.Events
            .Where(e => e.UserId == filter.UserId.Value && e.CreatedAt > filter.After)
            .OrderBy(e => e.CreatedAt)
            .SliceByLimitFilter(filter)
            .CountAndMapAsync(EventMapper.MapToDomainEntity, cancellationToken);
    }
}