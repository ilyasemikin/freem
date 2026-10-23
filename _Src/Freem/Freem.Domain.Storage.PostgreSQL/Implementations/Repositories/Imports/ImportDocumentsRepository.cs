using Freem.Domain.Entities.Import;
using Freem.Domain.Import;
using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Domain.Storage.Repositories.Imports;
using Microsoft.EntityFrameworkCore;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Imports;

internal sealed class ImportDocumentsRepository : IImportDocumentRepository
{
    private readonly DatabaseContext _database;
    private readonly DatabaseContextWriteExceptionHandler _exceptionHandler;

    public ImportDocumentsRepository(
        DatabaseContext database,
        DatabaseContextWriteExceptionHandler exceptionHandler)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(exceptionHandler);

        _database = database;
        _exceptionHandler = exceptionHandler;
    }

    public async Task<Result<Error>> CreateAsync(ImportDocument entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var dbEntity = entity.MapToDatabaseEntity();

        await _database.ImportDocuments.AddAsync(dbEntity, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<Error>> DeleteAsync(ImportDocumentId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var idString = id.ToString();
        var count = await _database.ImportDocuments
            .Where(e => e.Id == idString)
            .ExecuteDeleteAsync(cancellationToken);

        return count == 0
            ? DomainErrorCatalog.Factories.CreateEntityNotFound(id)
            : Result<Error>.CreateSuccess();
    }

    public async Task<Result<ImportDocument, Error>> FindByIdAsync(
        ImportDocumentId id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var idString = id.ToString();
        return await QueryDocuments().FindAsync(
            id,
            e => e.Id == idString,
            ImportDocumentMapper.MapToDomainEntity,
            cancellationToken);
    }

    public async Task<Result<ImportDocument, Error>> FindByMultipleIdAsync(
        ImportDocumentAndUserIds ids,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ids);

        var documentIdString = ids.DocumentId.ToString();
        var userIdString = ids.UserId.ToString();
        return await QueryDocuments().FindAsync(
            ids.DocumentId,
            e => e.Id == documentIdString && e.UserId == userIdString,
            ImportDocumentMapper.MapToDomainEntity,
            cancellationToken);
    }

    public async Task<Result<AsyncBatch<ImportDocument>, Error>> FindAsync(
        ImportDocumentByUserFilter filter,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var userIdString = filter.UserId.ToString();
        return await QueryDocuments()
            .AsNoTracking()
            .Where(e => e.UserId == userIdString)
            .OrderByDescending(e => e.CreatedAt)
            .ThenBy(e => e.Id)
            .CountAndMapAsync(filter, ImportDocumentMapper.MapToDomainEntity, cancellationToken);
    }

    public async Task<Result<Error>> ChangeStatusAsync(
        ImportDocumentId id,
        ImportDocumentStatus expected,
        ImportDocumentStatus next,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(expected);
        ArgumentNullException.ThrowIfNull(next);

        if (!expected.CanChangeTo(next))
            return DomainErrorCatalog.Factories.CreateImportInvalidStatus(id, expected, next);

        var idString = id.ToString();
        var expectedString = expected.ToString();
        var nextString = next.ToString();
        var now = DateTimeOffset.UtcNow;
        var count = await _database.ImportDocuments
            .Where(e => e.Id == idString && e.Status == expectedString)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(e => e.Status, nextString)
                    .SetProperty(e => e.UpdatedAt, now),
                cancellationToken);
        if (count != 0)
            return Result<Error>.CreateSuccess();

        var actual = await _database.ImportDocuments
            .AsNoTracking()
            .Where(e => e.Id == idString)
            .Select(e => e.Status)
            .FirstOrDefaultAsync(cancellationToken);
        if (actual is null)
            return DomainErrorCatalog.Factories.CreateEntityNotFound(id);

        return DomainErrorCatalog.Factories.CreateImportInvalidStatus(id, actual, next);
    }

    public async Task<Result<Error>> AddSourceAsync(
        ImportDocumentId id,
        IReadOnlyList<ImportDocumentTag> tags,
        IReadOnlyList<ImportDocumentActivity> activities,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(activities);

        var idString = id.ToString();
        foreach (var tag in tags)
        {
            var dbTag = tag.MapToDatabaseEntity(idString);
            await _database.ImportDocumentTags.AddAsync(dbTag, cancellationToken);
        }

        foreach (var activity in activities)
        {
            var dbActivity = activity.MapToDatabaseEntity(idString);
            await _database.ImportDocumentActivities.AddAsync(dbActivity, cancellationToken);
        }

        var context = new DatabaseContextWriteContext(id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<Error>> SetActivityResolutionAsync(
        ImportDocumentId id,
        ImportActivityId sourceId,
        ImportActivityResolution? resolution,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(sourceId);

        var idString = id.ToString();
        var sourceIdString = sourceId.ToString();
        var createNew = resolution is not null && resolution.IsCreateNew;
        var resolvedId = resolution?.ExistingId?.ToString();

        var count = 0;
        var context = new DatabaseContextWriteContext(id);
        var result = await _exceptionHandler.Handle(context, async () =>
        {
            count = await _database.ImportDocumentActivities
                .Where(e => e.DocumentId == idString && e.SourceId == sourceIdString)
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(e => e.CreateNew, createNew)
                        .SetProperty(e => e.ResolvedActivityId, resolvedId),
                    cancellationToken);
        });
        if (!result.Success)
            return result;

        return count == 0
            ? DomainErrorCatalog.Factories.CreateEntityNotFound(id)
            : Result<Error>.CreateSuccess();
    }

    public async Task<Result<Error>> SetTagResolutionAsync(
        ImportDocumentId id,
        ImportTagId sourceId,
        ImportTagResolution? resolution,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(sourceId);

        var idString = id.ToString();
        var sourceIdString = sourceId.ToString();
        var createNew = resolution is not null && resolution.IsCreateNew;
        var resolvedId = resolution?.ExistingId?.ToString();

        var count = 0;
        var context = new DatabaseContextWriteContext(id);
        var result = await _exceptionHandler.Handle(context, async () =>
        {
            count = await _database.ImportDocumentTags
                .Where(e => e.DocumentId == idString && e.SourceId == sourceIdString)
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(e => e.CreateNew, createNew)
                        .SetProperty(e => e.ResolvedTagId, resolvedId),
                    cancellationToken);
        });
        if (!result.Success)
            return result;

        return count == 0
            ? DomainErrorCatalog.Factories.CreateEntityNotFound(id)
            : Result<Error>.CreateSuccess();
    }

    public async Task<Result<Error>> AddRowsAsync(
        IReadOnlyList<ImportRow> rows,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(rows);

        if (rows.Count == 0)
            return Result<Error>.CreateSuccess();

        var dbRows = rows
            .Select(r => r.MapToDatabaseEntity())
            .ToList();
        await _database.ImportRows.AddRangeAsync(dbRows, cancellationToken);

        var context = new DatabaseContextWriteContext(rows[0].DocumentId);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<ImportRow, Error>> FindRowByIdAsync(
        ImportRowId id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var idString = id.ToString();
        return await QueryRows().FindAsync(
            id,
            e => e.Id == idString,
            ImportRowMapper.MapToDomainEntity,
            cancellationToken);
    }

    public async Task<Result<AsyncBatch<ImportRow>, Error>> FindRowsAsync(
        ImportRowByDocumentFilter filter,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var documentIdString = filter.DocumentId.ToString();
        return await QueryRows()
            .AsNoTracking()
            .Where(e => e.DocumentId == documentIdString)
            .OrderBy(e => e.FirstStartAt)
            .ThenBy(e => e.Id)
            .CountAndMapAsync(filter, ImportRowMapper.MapToDomainEntity, cancellationToken);
    }

    private IQueryable<ImportDocumentEntity> QueryDocuments()
    {
        return _database.ImportDocuments
            .Include(e => e.Tags)
            .Include(e => e.Activities)
            .Include($"{nameof(ImportDocumentEntity.Activities)}.{nameof(ImportDocumentActivityEntity.Tags)}")
            .AsSplitQuery();
    }

    private IQueryable<ImportRowEntity> QueryRows()
    {
        return _database.ImportRows
            .Include(e => e.Periods)
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .AsSplitQuery();
    }
}
