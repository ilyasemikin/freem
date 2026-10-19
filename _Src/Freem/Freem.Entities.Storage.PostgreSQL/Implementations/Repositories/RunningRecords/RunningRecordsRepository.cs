using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.RunningRecords;
using Freem.Domain.Storage.Exceptions;
using Freem.Domain.Storage.Models;
using Freem.Domain.Storage.Repositories.RunningRecords;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;

internal sealed class RunningRecordsRepository : IRunningRecordRepository
{
    private readonly DatabaseContext _database;
    private readonly DatabaseContextWriteExceptionHandler _exceptionHandler;
    private readonly IEqualityComparer<RunningRecord> _equalityComparer;

    public RunningRecordsRepository(
        DatabaseContext database,
        DatabaseContextWriteExceptionHandler exceptionHandler,
        IEqualityComparer<RunningRecord> equalityComparer)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(exceptionHandler);
        ArgumentNullException.ThrowIfNull(equalityComparer);
        
        _database = database;
        _exceptionHandler = exceptionHandler;
        _equalityComparer = equalityComparer;
    }

    public async Task CreateAsync(RunningRecord entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = entity.MapToDatabaseEntity();
        var dbActivityRelations = entity.CreateDatabaseRunningRecordActivityRelations();
        var dbTagRelations = entity.CreateDatabaseRunningRecordTagRelations();

        await _database.RunningRecords.AddAsync(dbEntity, cancellationToken);
        await _database.AddRangeAsync(dbActivityRelations, cancellationToken);
        await _database.AddRangeAsync(dbTagRelations, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task UpdateAsync(RunningRecord entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = await _database.RunningRecords.FindEntityAsync(entity, cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(entity.Id);

        var actualEntity = dbEntity.MapToDomainEntity();
        if (_equalityComparer.Equals(entity, actualEntity))
            return;
        
        dbEntity.Name = entity.Name?.ToString();
        dbEntity.Description = entity.Description?.ToString();

        dbEntity.StartAt = entity.StartAt;

        await _database.UpdateRelatedActivitiesAsync(entity, cancellationToken);
        await _database.UpdateRelatedTagsAsync(entity, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task DeleteAsync(RunningRecordId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var userId = id.ToUserId();
        var dbEntity = await _database.RunningRecords.FindEntityAsync(userId, cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(id);

        _database.Remove(dbEntity);

        var context = new DatabaseContextWriteContext(id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<SearchEntityResult<RunningRecord>> FindByIdAsync(
        RunningRecordId id,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var idString = id.ToString();
        return await _database.RunningRecords
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FindAsync(e => e.UserId == idString, RunningRecordMapper.MapToDomainEntity, cancellationToken);
    }
}