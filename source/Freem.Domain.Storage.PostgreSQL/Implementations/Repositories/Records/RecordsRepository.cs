using Freem.Domain.Entities.Records;
using Freem.Domain.Entities.Records.Comparers;
using Freem.Domain.Records;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;
using Freem.Domain.Storage.PostgreSQL.Database.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Records.Extensions;
using Microsoft.EntityFrameworkCore;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Records;

internal sealed class RecordsRepository : IRecordRepository
{
    private readonly DatabaseContext _database;
    private readonly DatabaseContextWriteExceptionHandler _exceptionHandler;

    public RecordsRepository(
        DatabaseContext database,
        DatabaseContextWriteExceptionHandler exceptionHandler)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(exceptionHandler);

        _database = database;
        _exceptionHandler = exceptionHandler;
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

    public async Task<Result<Error>> CreateManyAsync(
        IReadOnlyList<Record> entities,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        if (entities.Count == 0)
            return Result<Error>.CreateSuccess();

        foreach (var entity in entities)
        {
            var dbEntity = entity.MapToDatabaseEntity();
            var dbActivityRelations = entity.CreateDatabaseRecordActivityRelations();
            var dbTagRelations = entity.CreateDatabaseRecordTagRelations();

            await _database.Records.AddAsync(dbEntity, cancellationToken);
            await _database.AddRangeAsync(dbActivityRelations, cancellationToken);
            await _database.AddRangeAsync(dbTagRelations, cancellationToken);
        }

        var context = new DatabaseContextWriteContext(entities[0].Id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<Error>> UpdateAsync(Record entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var dbEntity = await _database.Records.FindEntityAsync(entity, cancellationToken);
        if (dbEntity is null)
            return DomainErrorCatalog.Factories.CreateRecordNotFound(entity.Id);

        var actualEntity = dbEntity.MapToDomainEntity();
        if (RecordEqualityComparer.Instance.Equals(entity, actualEntity))
            return Result<Error>.CreateSuccess();

        dbEntity.Name = entity.Name?.ToString();
        dbEntity.Description = entity.Description?.ToString();

        UpdatePeriods(dbEntity, entity);

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
            return DomainErrorCatalog.Factories.CreateRecordNotFound(id);

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
            .Include(e => e.Periods)
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FindAsync(
                () => DomainErrorCatalog.Factories.CreateRecordNotFound(id),
                e => e.Id == idString,
                RecordMapper.MapToDomainEntity,
                cancellationToken);
    }

    public async Task<Result<Record, Error>> FindByMultipleIdAsync(
        RecordAndUserIds ids,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ids);

        var recordIdString = ids.RecordId.ToString();
        var userIdString = ids.UserId.ToString();
        return await _database.Records
            .Include(e => e.Periods)
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FindAsync(
                () => DomainErrorCatalog.Factories.CreateRecordNotFound(ids.RecordId),
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
            .Include(e => e.Periods)
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .Where(e => e.UserId == userIdString)
            .OrderBy(e => e.Periods!.Min(p => p.StartAt))
            .ThenBy(e => e.Periods!.Max(p => p.EndAt))
            .ThenBy(e => e.Id)
            .AsNoTracking()
            .CountAndMapAsync(filter, RecordMapper.MapToDomainEntity, cancellationToken);
    }

    public async Task<Result<AsyncBatch<Record>, Error>> FindAsync(
        RecordByPeriodFilter filter,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var userIdString = filter.UserId.ToString();
        var startAt = filter.Period.StartAt;
        var endAt = filter.Period.EndAt;

        var matching = _database.Records
            .Include(e => e.Periods)
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .Where(e =>
                e.UserId == userIdString &&
                e.Periods!.Any(p => p.StartAt < endAt && p.EndAt > startAt))
            .OrderBy(e => e.Periods!.Min(p => p.StartAt))
            .ThenBy(e => e.Periods!.Max(p => p.EndAt))
            .ThenBy(e => e.Id)
            .AsNoTracking();

        var page = matching;
        if (filter.AfterRecordId is not null)
        {
            var afterRecordId = filter.AfterRecordId.ToString();
            var cursor = await _database.Records
                .Where(e => e.Id == afterRecordId && e.UserId == userIdString)
                .Select(e => new
                {
                    StartAt = e.Periods!.Min(p => p.StartAt),
                    EndAt = e.Periods!.Max(p => p.EndAt)
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (cursor is null)
                return DomainErrorCatalog.Factories.CreateRecordNotFound(filter.AfterRecordId);

            var cursorStartAt = cursor.StartAt;
            var cursorEndAt = cursor.EndAt;
            page = matching.Where(e =>
                e.Periods!.Min(p => p.StartAt) > cursorStartAt ||
                e.Periods!.Min(p => p.StartAt) == cursorStartAt && e.Periods!.Max(p => p.EndAt) > cursorEndAt ||
                e.Periods!.Min(p => p.StartAt) == cursorStartAt && e.Periods!.Max(p => p.EndAt) == cursorEndAt &&
                string.Compare(e.Id, afterRecordId) > 0);
        }

        return await matching.CountAndMapAsync(page, filter, RecordMapper.MapToDomainEntity, cancellationToken);
    }

    private void UpdatePeriods(RecordEntity dbEntity, Record entity)
    {
        var dbPeriods = dbEntity.Periods ??= [];
        var recordId = dbEntity.Id;

        var actualIds = entity.Periods
            .Select(p => p.Id.ToString())
            .ToHashSet();

        var removed = dbPeriods
            .Where(p => !actualIds.Contains(p.Id))
            .ToArray();
        foreach (var dbPeriod in removed)
            dbPeriods.Remove(dbPeriod);

        foreach (var period in entity.Periods)
        {
            var id = period.Id.ToString();
            var dbPeriod = dbPeriods.FirstOrDefault(p => p.Id == id);
            if (dbPeriod is null)
            {
                var added = period.MapToDatabaseEntity(recordId);
                _database.Add(added);
                continue;
            }

            dbPeriod.StartAt = period.Period.StartAt;
            dbPeriod.EndAt = period.Period.EndAt;
            dbPeriod.TimeZoneOffsetTicks = period.TimeZoneOffset.Ticks;
            dbPeriod.Description = period.Description?.ToString();
        }
    }
}
