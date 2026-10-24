using Freem.Domain.Activities;
using Freem.Domain.Entities.DataTransfer.Import;
using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Domain.Storage.Repositories.DataTransfer.Import;
using Freem.Domain.Tags;
using Microsoft.EntityFrameworkCore;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.DataTransfer.Import;

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
            ? DomainErrorCatalog.Factories.CreateImportDocumentNotFound(id)
            : Result<Error>.CreateSuccess();
    }

    public async Task<Result<ImportDocument, Error>> FindByIdAsync(
        ImportDocumentId id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var idString = id.ToString();
        return await QueryDocuments().FindAsync(
            () => DomainErrorCatalog.Factories.CreateImportDocumentNotFound(id),
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
            () => DomainErrorCatalog.Factories.CreateImportDocumentNotFound(ids.DocumentId),
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
            return DomainErrorCatalog.Factories.CreateImportDocumentNotFound(id);

        return DomainErrorCatalog.Factories.CreateImportInvalidStatus(id, actual, next);
    }

    public async Task<Result<Error>> AddTagsAndActivitiesAsync(
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

    public async Task<Result<AsyncBatch<ImportDocumentActivity>, Error>> FindActivitiesAsync(
        ImportDocumentActivityByDocumentFilter filter,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var documentIdString = filter.DocumentId.ToString();
        return await _database.ImportDocumentActivities
            .Include(e => e.Tags)
            .AsNoTracking()
            .Where(e => e.DocumentId == documentIdString)
            .OrderBy(e => e.ImportActivityId)
            .CountAndMapAsync(filter, ImportDocumentMapper.MapToDomainEntity, cancellationToken);
    }

    public async Task<Result<AsyncBatch<ImportDocumentTag>, Error>> FindTagsAsync(
        ImportDocumentTagByDocumentFilter filter,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var documentIdString = filter.DocumentId.ToString();
        return await _database.ImportDocumentTags
            .AsNoTracking()
            .Where(e => e.DocumentId == documentIdString)
            .OrderBy(e => e.ImportTagId)
            .CountAndMapAsync(filter, ImportDocumentMapper.MapToDomainEntity, cancellationToken);
    }

    public async Task<Result<Error>> UpdateActivityMappingAsync(
        ImportDocumentId id,
        ImportActivityId importActivityId,
        ActivityId? activityId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(importActivityId);

        var idString = id.ToString();
        var importActivityIdString = importActivityId.ToString();
        var mappedIdString = activityId?.ToString();

        var count = 0;
        var context = new DatabaseContextWriteContext(id);
        var result = await _exceptionHandler.Handle(context, async () =>
        {
            count = await _database.ImportDocumentActivities
                .Where(e => e.DocumentId == idString && e.ImportActivityId == importActivityIdString)
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(e => e.MappedActivityId, mappedIdString),
                    cancellationToken);
        });
        if (!result.Success)
            return result;

        return count == 0
            ? DomainErrorCatalog.Factories.CreateImportActivityNotFound(importActivityId)
            : Result<Error>.CreateSuccess();
    }

    public async Task<Result<Error>> UpdateTagMappingAsync(
        ImportDocumentId id,
        ImportTagId importTagId,
        TagId? tagId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(importTagId);

        var idString = id.ToString();
        var importTagIdString = importTagId.ToString();
        var mappedIdString = tagId?.ToString();

        var count = 0;
        var context = new DatabaseContextWriteContext(id);
        var result = await _exceptionHandler.Handle(context, async () =>
        {
            count = await _database.ImportDocumentTags
                .Where(e => e.DocumentId == idString && e.ImportTagId == importTagIdString)
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(e => e.MappedTagId, mappedIdString),
                    cancellationToken);
        });
        if (!result.Success)
            return result;

        return count == 0
            ? DomainErrorCatalog.Factories.CreateImportTagNotFound(importTagId)
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
            () => DomainErrorCatalog.Factories.CreateImportRowNotFound(id),
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
        var idStrings = filter.Ids?
            .Select(id => id.ToString())
            .ToArray();
        return await QueryRows()
            .AsNoTracking()
            .Where(e => e.DocumentId == documentIdString)
            .Where(e => idStrings == null || idStrings.Contains(e.Id))
            .OrderBy(e => e.FirstStartAt)
            .ThenBy(e => e.Id)
            .CountAndMapAsync(filter, ImportRowMapper.MapToDomainEntity, cancellationToken);
    }

    private IQueryable<ImportDocumentEntity> QueryDocuments()
    {
        return _database.ImportDocuments;
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
