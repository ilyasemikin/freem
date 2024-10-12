using Freem.Entities.Records;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
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
    private readonly IEqualityComparer<Record> _equalityComparer;

    public RecordsRepository(
        DatabaseContext database,
        DatabaseContextWriteExceptionHandler exceptionHandler,
        IEqualityComparer<Record> equalityComparer)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(exceptionHandler);
        ArgumentNullException.ThrowIfNull(equalityComparer);
        
        _database = database;
        _exceptionHandler = exceptionHandler;
        _equalityComparer = equalityComparer;
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

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task UpdateAsync(Record entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = await _database.Records.FindEntityAsync(entity, cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(entity.Id);

        var actualEntity = dbEntity.MapToDomainEntity();
        if (_equalityComparer.Equals(entity, actualEntity))
            return;
        
        dbEntity.Name = entity.Name;
        dbEntity.Description = entity.Description;

        dbEntity.StartAt = entity.Period.StartAt;
        dbEntity.EndAt = entity.Period.EndAt;

        await _database.UpdateRelatedActivitiesAsync(entity, cancellationToken);
        await _database.UpdateRelatedTagsAsync(entity, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task RemoveAsync(RecordIdentifier id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var dbEntity = await _database.Records.FindEntityAsync(id, cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(id);

        _database.Remove(dbEntity);
        
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
            .FindAsync(e => e.Id == id, RecordMapper.MapToDomainEntity, cancellationToken);
    }
    
    public async Task<SearchEntityResult<Record>> FindByMultipleIdAsync(
        RecordAndUserIdentifiers ids,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ids);
        
        return await _database.Records
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FindAsync(
                e => e.Id == ids.RecordId && e.UserId == ids.UserId, 
                RecordMapper.MapToDomainEntity,
                cancellationToken);
    }

    public async Task<SearchEntitiesAsyncResult<Record>> FindAsync(
        RecordsByUserFilter filter, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);
        
        return await _database.Records
            .Where(e => e.UserId == filter.UserId)
            .OrderBy(e => e.StartAt)
            .SliceByLimitAndOffsetFilter(filter)
            .CountAndMapAsync(RecordMapper.MapToDomainEntity, cancellationToken);
    }
}