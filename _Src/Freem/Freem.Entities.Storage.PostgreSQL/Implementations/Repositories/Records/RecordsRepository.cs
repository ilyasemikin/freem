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
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records;

internal sealed class RecordsRepository : IRecordsRepository
{
    private readonly DatabaseContext _context;
    private readonly DatabaseContextExceptionHandler _exceptionHandler;
    private readonly IEventEntityFactory<RecordEvent, Record> _eventFactory;

    public RecordsRepository(
        DatabaseContext context,
        DatabaseContextExceptionHandler exceptionHandler,
        IEventEntityFactory<RecordEvent, Record> eventFactory)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(exceptionHandler);
        ArgumentNullException.ThrowIfNull(eventFactory);
        
        _context = context;
        _exceptionHandler = exceptionHandler;
        _eventFactory = eventFactory;
    }

    public async Task CreateAsync(Record entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = entity.MapToDatabaseEntity();
        var dbActivityRelations = entity.CreateDatabaseRecordActivityRelations();
        var dbTagRelations = entity.CreateDatabaseRecordTagRelations();

        await _context.Records.AddAsync(dbEntity, cancellationToken);
        await _context.AddRangeAsync(dbActivityRelations, cancellationToken);
        await _context.AddRangeAsync(dbTagRelations, cancellationToken);

        await WriteEventAsync(entity, EventAction.Created, cancellationToken);

        await _exceptionHandler.HandleSaveChangesAsync(_context, cancellationToken);
    }

    public async Task UpdateAsync(Record entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = await _context.Records.FirstOrDefaultAsync(
            e => e.Id == entity.Id.Value && e.UserId == entity.UserId.Value,
            cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(entity.Id);

        dbEntity.Name = entity.Name;
        dbEntity.Description = entity.Description;

        dbEntity.StartAt = entity.Period.StartAt;
        dbEntity.EndAt = entity.Period.EndAt;

        await _context.UpdateRelatedActivitiesAsync(entity, cancellationToken);
        await _context.UpdateRelatedTagsAsync(entity, cancellationToken);

        await WriteEventAsync(entity, EventAction.Updated, cancellationToken);

        await _exceptionHandler.HandleSaveChangesAsync(_context, cancellationToken);
    }

    public async Task RemoveAsync(RecordIdentifier id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var dbEntity = await _context.Records.FirstOrDefaultAsync(e => e.Id == id.Value, cancellationToken);

        if (dbEntity is null)
            throw new NotFoundException(id);

        _context.Remove(dbEntity);
        
        var entity = dbEntity.MapToDomainEntity();
        await WriteEventAsync(entity, EventAction.Removed, cancellationToken);

        await _exceptionHandler.HandleSaveChangesAsync(_context, cancellationToken);
    }

    public async Task<SearchEntityResult<Record>> FindByIdAsync(
        RecordIdentifier id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        return await _context.Records
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FindAsync(e => e.Id == id.Value, RecordMapper.MapToDomainEntity, cancellationToken);
    }
    
    public async Task<SearchEntityResult<Record>> FindAsync(
        RecordAndUserIdentifiers ids,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ids);
        
        return await _context.Records
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FindAsync(
                e => e.Id == ids.RecordId.Value && e.UserId == ids.UserId.Value, 
                RecordMapper.MapToDomainEntity,
                cancellationToken);
    }

    public async Task<SearchEntitiesAsyncResult<Record>> FindByUserAsync(
        RecordsByUserFilter filter, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);
        
        return await _context.Records
            .Where(e => e.UserId == filter.UserId.Value)
            .OrderBy(e => e.StartAt)
            .SliceByLimitAndOffsetFilter(filter)
            .CountAndMapAsync(RecordMapper.MapToDomainEntity, cancellationToken);
    }

    private async Task WriteEventAsync(Record entity, EventAction action, CancellationToken cancellationToken)
    {
        var eventEntity = _eventFactory.Create(entity, action);
        var dbEventEntity = eventEntity.MapToDatabaseEntity();
        await _context.Events.AddAsync(dbEventEntity, cancellationToken);
    }
}