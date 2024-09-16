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
    private readonly DatabaseContext _database;
    private readonly DatabaseContextWriteExceptionHandler _exceptionHandler;
    private readonly IEventEntityFactory<RecordEvent, Record> _eventFactory;

    public RecordsRepository(
        DatabaseContext database,
        DatabaseContextWriteExceptionHandler exceptionHandler,
        IEventEntityFactory<RecordEvent, Record> eventFactory)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(exceptionHandler);
        ArgumentNullException.ThrowIfNull(eventFactory);
        
        _database = database;
        _exceptionHandler = exceptionHandler;
        _eventFactory = eventFactory;
    }

    public async Task CreateAsync(Record entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = entity.MapToDatabaseEntity();
        var dbActivityRelations = entity.CreateDatabaseRecordActivityRelations();
        var dbTagRelations = entity.CreateDatabaseRecordTagRelations();

        await _database.Records.AddAsync(dbEntity, cancellationToken);
        await _database.AddRangeAsync(dbActivityRelations, cancellationToken);
        await _database.AddRangeAsync(dbTagRelations, cancellationToken);

        await WriteEventAsync(entity, EventAction.Created, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task UpdateAsync(Record entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = await _database.Records.FirstOrDefaultAsync(
            e => e.Id == entity.Id.Value && e.UserId == entity.UserId.Value,
            cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(entity.Id);

        dbEntity.Name = entity.Name;
        dbEntity.Description = entity.Description;

        dbEntity.StartAt = entity.Period.StartAt;
        dbEntity.EndAt = entity.Period.EndAt;

        await _database.UpdateRelatedActivitiesAsync(entity, cancellationToken);
        await _database.UpdateRelatedTagsAsync(entity, cancellationToken);

        await WriteEventAsync(entity, EventAction.Updated, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task RemoveAsync(RecordIdentifier id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var dbEntity = await _database.Records
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.Id == id.Value, cancellationToken);

        if (dbEntity is null)
            throw new NotFoundException(id);

        _database.Remove(dbEntity);
        
        var entity = dbEntity.MapToDomainEntity();
        await WriteEventAsync(entity, EventAction.Removed, cancellationToken);

        var context = new DatabaseContextWriteContext(id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<SearchEntityResult<Record>> FindByIdAsync(
        RecordIdentifier id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        return await _database.Records
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FindAsync(e => e.Id == id.Value, RecordMapper.MapToDomainEntity, cancellationToken);
    }
    
    public async Task<SearchEntityResult<Record>> FindAsync(
        RecordAndUserIdentifiers ids,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ids);
        
        return await _database.Records
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
        
        return await _database.Records
            .Where(e => e.UserId == filter.UserId.Value)
            .OrderBy(e => e.StartAt)
            .SliceByLimitAndOffsetFilter(filter)
            .CountAndMapAsync(RecordMapper.MapToDomainEntity, cancellationToken);
    }

    private async Task WriteEventAsync(Record entity, EventAction action, CancellationToken cancellationToken)
    {
        var eventEntity = _eventFactory.Create(entity, action);
        var dbEventEntity = eventEntity.MapToDatabaseEntity();
        await _database.Events.AddAsync(dbEventEntity, cancellationToken);
    }
}