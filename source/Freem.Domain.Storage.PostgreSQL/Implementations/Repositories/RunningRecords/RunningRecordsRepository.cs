using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.Entities.RunningRecords.Comparers;
using Freem.Domain.RunningRecords;
using Freem.Domain.Storage.Repositories.RunningRecords;
using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.Database.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.RunningRecords.Extensions;
using Microsoft.EntityFrameworkCore;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;

internal sealed class RunningRecordsRepository : IRunningRecordRepository
{
    private readonly DatabaseContext _database;
    private readonly DatabaseContextWriteExceptionHandler _exceptionHandler;

    public RunningRecordsRepository(
        DatabaseContext database,
        DatabaseContextWriteExceptionHandler exceptionHandler)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(exceptionHandler);
        
        _database = database;
        _exceptionHandler = exceptionHandler;
    }

    public async Task<Result<Error>> CreateAsync(RunningRecord entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = entity.MapToDatabaseEntity();
        var dbActivityRelations = entity.CreateDatabaseRunningRecordActivityRelations();
        var dbTagRelations = entity.CreateDatabaseRunningRecordTagRelations();

        await _database.RunningRecords.AddAsync(dbEntity, cancellationToken);
        await _database.AddRangeAsync(dbActivityRelations, cancellationToken);
        await _database.AddRangeAsync(dbTagRelations, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<Error>> UpdateAsync(RunningRecord entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = await _database.RunningRecords.FindEntityAsync(entity, cancellationToken);
        if (dbEntity is null)
            return DomainErrorCatalog.Factories.CreateRunningRecordNotFound(entity.Id);

        var actualEntity = dbEntity.MapToDomainEntity();
        if (RunningRecordEqualityComparer.Instance.Equals(entity, actualEntity))
            return Result<Error>.CreateSuccess();
        
        dbEntity.Name = entity.Name?.ToString();
        dbEntity.Description = entity.Description?.ToString();

        dbEntity.StartAt = entity.StartAt;

        await _database.UpdateRelatedActivitiesAsync(entity, cancellationToken);
        await _database.UpdateRelatedTagsAsync(entity, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<Error>> DeleteAsync(RunningRecordId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var userId = id.ToUserId();
        var dbEntity = await _database.RunningRecords.FindEntityAsync(userId, cancellationToken);
        if (dbEntity is null)
            return DomainErrorCatalog.Factories.CreateRunningRecordNotFound(id);

        _database.Remove(dbEntity);

        var context = new DatabaseContextWriteContext(id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<RunningRecord, Error>> FindByIdAsync(
        RunningRecordId id,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var idString = id.ToString();
        return await _database.RunningRecords
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FindAsync(
                () => DomainErrorCatalog.Factories.CreateRunningRecordNotFound(id),
                e => e.UserId == idString,
                RunningRecordMapper.MapToDomainEntity,
                cancellationToken);
    }
}