using System.Linq.Expressions;
using Freem.Domain;
using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records.Extensions;
using Microsoft.EntityFrameworkCore;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records;

internal sealed class RecordsRepository : IRecordRepository
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

    public async Task<Result<Error>> CreateAsync(Record entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var dbEntity = entity.MapToDatabaseEntity();
        var dbActivityRelations = entity.CreateDatabaseRecordActivityRelations();
        var dbTagRelations = entity.CreateDatabaseRecordTagRelations();

        await _database.Records.AddAsync(dbEntity, cancellationToken);
        await _database.AddRangeAsync(dbActivityRelations, cancellationToken);
        await _database.AddRangeAsync(dbTagRelations, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<Error>> UpdateAsync(Record entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var dbEntity = await _database.Records.FindEntityAsync(entity, cancellationToken);
        if (dbEntity is null)
            return DomainErrorCatalog.Factories.CreateEntityNotFound(entity.Id);

        var actualEntity = dbEntity.MapToDomainEntity();
        if (_equalityComparer.Equals(entity, actualEntity))
            return Result<Error>.CreateSuccess();

        dbEntity.Name = entity.Name?.ToString();
        dbEntity.Description = entity.Description?.ToString();

        dbEntity.StartAt = entity.Period.StartAt;
        dbEntity.EndAt = entity.Period.EndAt;

        await _database.UpdateRelatedActivitiesAsync(entity, cancellationToken);
        await _database.UpdateRelatedTagsAsync(entity, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<Error>> DeleteAsync(RecordId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var dbEntity = await _database.Records.FindEntityAsync(id, cancellationToken);
        if (dbEntity is null)
            return DomainErrorCatalog.Factories.CreateEntityNotFound(id);

        _database.Remove(dbEntity);

        var context = new DatabaseContextWriteContext(id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<Record, Error>> FindByIdAsync(
        RecordId id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var idString = id.ToString();
        return await _database.Records
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FindAsync(id, e => e.Id == idString, RecordMapper.MapToDomainEntity, cancellationToken);
    }

    public async Task<Result<Record, Error>> FindByMultipleIdAsync(
        RecordAndUserIds ids,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ids);

        var recordIdString = ids.RecordId.ToString();
        var userIdString = ids.UserId.ToString();
        return await _database.Records
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FindAsync(
                ids.RecordId,
                e => e.Id == recordIdString && e.UserId == userIdString,
                RecordMapper.MapToDomainEntity,
                cancellationToken);
    }

    public async Task<Result<AsyncBatch<Record>, Error>> FindAsync(
        RecordByUserFilter filter,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var userIdString = filter.UserId.ToString();
        return await _database.Records
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .Where(e => e.UserId == userIdString)
            .OrderBy(e => e.StartAt)
            .ThenBy(e => e.EndAt)
            .ThenBy(e => e.Id)
            .SliceByLimitAndOffsetFilter(filter)
            .AsNoTracking()
            .CountAndMapAsync(RecordMapper.MapToDomainEntity, cancellationToken);
    }

    public async Task<Result<AsyncBatch<Record>, Error>> FindAsync(
        RecordByPeriodFilter filter,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var userIdString = filter.UserId.ToString();

        async Task<Result<AsyncBatch<Record>, Error>> FindInnerAsync(Expression<Func<RecordEntity, bool>> predicate)
        {
            return await _database.Records
                .Include(e => e.Activities)
                .Include(e => e.Tags)
                .Where(predicate)
                .OrderBy(e => e.StartAt)
                .ThenBy(e => e.EndAt)
                .ThenBy(e => e.Id)
                .SliceByLimitFilter(filter)
                .AsNoTracking()
                .CountAndMapAsync(RecordMapper.MapToDomainEntity, cancellationToken);
        }

        if (filter.AfterRecordId is null)
            return await FindInnerAsync(e =>
                e.UserId == userIdString &&
                (filter.Period.StartAt <= e.StartAt && e.StartAt < filter.Period.EndAt ||
                 filter.Period.StartAt <= e.EndAt && e.EndAt < filter.Period.EndAt));

        var afterRecordId = filter.AfterRecordId.ToString();
        return await FindInnerAsync(e =>
            e.UserId == userIdString &&
            (filter.Period.StartAt <= e.StartAt && e.StartAt < filter.Period.EndAt ||
             filter.Period.StartAt <= e.EndAt && e.EndAt < filter.Period.EndAt) &&
            (e.StartAt > filter.Period.StartAt ||
             e.StartAt == filter.Period.StartAt && e.EndAt > filter.Period.EndAt ||
             e.StartAt == filter.Period.EndAt && e.EndAt == filter.Period.EndAt && string.Compare(e.Id, afterRecordId) > 0)
            );
    }
}