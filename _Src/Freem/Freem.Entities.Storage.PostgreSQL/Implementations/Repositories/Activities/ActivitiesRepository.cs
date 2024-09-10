using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Factories;
using Freem.Entities.Events;
using Freem.Entities.Identifiers;
using Freem.Entities.Identifiers.Multiple;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Activities.Extensions;
using Freem.Sorting.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Activities;

internal sealed class ActivitiesRepository : IActivitiesRepository
{
    private readonly DatabaseContext _context;
    private readonly DatabaseContextExceptionHandler _exceptionHandler;
    private readonly IEventEntityFactory<ActivityEvent, Activity> _eventFactory;

    public ActivitiesRepository(
        DatabaseContext context,
        DatabaseContextExceptionHandler exceptionHandler,
        IEventEntityFactory<ActivityEvent, Activity> eventFactory)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(exceptionHandler);
        ArgumentNullException.ThrowIfNull(eventFactory);
        
        _context = context;
        _exceptionHandler = exceptionHandler;
        _eventFactory = eventFactory;
    }

    public async Task CreateAsync(Activity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = entity.MapToDatabaseEntity();
        var dbRelations = entity.CreateDatabaseActivityTagRelations();

        await _context.Activities.AddAsync(dbEntity, cancellationToken);
        await _context.AddRangeAsync(dbRelations, cancellationToken);

        await WriteEventAsync(entity, EventAction.Created, cancellationToken);

        await _exceptionHandler.HandleSaveChangesAsync(_context, cancellationToken);
    }

    public async Task UpdateAsync(Activity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = await _context.Activities.FirstOrDefaultAsync(
            e => e.Id == entity.Id.Value && e.UserId == entity.UserId.Value,
            cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(entity.Id);
        
        dbEntity.Name = entity.Name;
        dbEntity.Status = entity.Status.MapToDatabaseEntityStatus();

        await _context.UpdateRelatedTagsAsync(entity, cancellationToken);

        await WriteEventAsync(entity, EventAction.Updated, cancellationToken);

        await _exceptionHandler.HandleSaveChangesAsync(_context, cancellationToken);
    }

    public async Task RemoveAsync(ActivityIdentifier id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var dbEntity = await _context.Activities.FirstOrDefaultAsync(e => e.Id == id.Value, cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(id);

        _context.Remove(dbEntity);
        
        var entity = dbEntity.MapToDomainEntity();
        await WriteEventAsync(entity, EventAction.Removed, cancellationToken);

        await _exceptionHandler.HandleSaveChangesAsync(_context, cancellationToken);
    }

    public async Task<SearchEntityResult<Activity>> FindByIdAsync(
        ActivityIdentifier id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        return await _context.Activities
            .Include(e => e.Tags)
            .FindAsync(e => e.Id == id.Value, ActivityMapper.MapToDomainEntity, cancellationToken);
    }

    public async Task<SearchEntityResult<Activity>> FindAsync(ActivityAndUserIdentifiers ids, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(ids);
        
        return await _context.Activities
            .Include(e => e.Tags)
            .FindAsync(
                e => e.Id == ids.ActivityId.Value && e.UserId == ids.UserId.Value,
                ActivityMapper.MapToDomainEntity, 
                cancellationToken);
    }

    public async Task<SearchEntitiesAsyncResult<Activity>> FindByUserAsync(ActivitiesByUserFilter filter, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);
        
        return await _context.Activities
            .Where(e => e.UserId == filter.UserId.Value)
            .OrderBy(filter.Sorting, ActivityFactories.CreateSortSelector)
            .SliceByLimitAndOffsetFilter(filter)
            .CountAndMapAsync(ActivityMapper.MapToDomainEntity, cancellationToken);
    }

    private async Task WriteEventAsync(Activity entity, EventAction action, CancellationToken cancellationToken)
    {
        var eventEntity = _eventFactory.Create(entity, action);
        var dbEventEntity = eventEntity.MapToDatabaseEntity();
        await _context.Events.AddAsync(dbEventEntity, cancellationToken);
    }
}