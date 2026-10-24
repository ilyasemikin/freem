using Freem.Domain.Activities;
using Freem.Domain.Entities.DataTransfer.Import;
using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Tags;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.Repositories.DataTransfer.Import;

public interface IImportDocumentRepository
{
    Task<Result<Error>> CreateAsync(ImportDocument entity, CancellationToken cancellationToken = default);
    Task<Result<Error>> DeleteAsync(ImportDocumentId id, CancellationToken cancellationToken = default);

    Task<Result<ImportDocument, Error>> FindByIdAsync(
        ImportDocumentId id,
        CancellationToken cancellationToken = default);
    Task<Result<ImportDocument, Error>> FindByMultipleIdAsync(
        ImportDocumentAndUserIds ids,
        CancellationToken cancellationToken = default);
    Task<Result<AsyncBatch<ImportDocument>, Error>> FindAsync(
        ImportDocumentByUserFilter filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Atomically changes the status if the current one equals <paramref name="expected"/>
    /// and the transition is allowed.
    /// </summary>
    Task<Result<Error>> ChangeStatusAsync(
        ImportDocumentId id,
        ImportDocumentStatus expected,
        ImportDocumentStatus next,
        CancellationToken cancellationToken = default);

    Task<Result<Error>> AddTagsAndActivitiesAsync(
        ImportDocumentId id,
        IReadOnlyList<ImportDocumentTag> tags,
        IReadOnlyList<ImportDocumentActivity> activities,
        CancellationToken cancellationToken = default);

    Task<Result<AsyncBatch<ImportDocumentActivity>, Error>> FindActivitiesAsync(
        ImportDocumentActivityByDocumentFilter filter,
        CancellationToken cancellationToken = default);
    Task<Result<AsyncBatch<ImportDocumentTag>, Error>> FindTagsAsync(
        ImportDocumentTagByDocumentFilter filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the mapping of an import activity to an existing Freem activity; null removes the mapping.
    /// </summary>
    Task<Result<Error>> UpdateActivityMappingAsync(
        ImportDocumentId id,
        ImportActivityId importActivityId,
        ActivityId? activityId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// Updates the mapping of an import tag to an existing Freem tag; null removes the mapping.
    /// </summary>
    Task<Result<Error>> UpdateTagMappingAsync(
        ImportDocumentId id,
        ImportTagId importTagId,
        TagId? tagId,
        CancellationToken cancellationToken = default);

    Task<Result<Error>> AddRowsAsync(
        IReadOnlyList<ImportRow> rows,
        CancellationToken cancellationToken = default);
    Task<Result<ImportRow, Error>> FindRowByIdAsync(
        ImportRowId id,
        CancellationToken cancellationToken = default);
    Task<Result<AsyncBatch<ImportRow>, Error>> FindRowsAsync(
        ImportRowByDocumentFilter filter,
        CancellationToken cancellationToken = default);
}
