using Freem.Domain.Entities.Import;
using Freem.Domain.Import;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.Repositories.Imports;

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

    Task<Result<Error>> AddSourceAsync(
        ImportDocumentId id,
        IReadOnlyList<ImportDocumentTag> tags,
        IReadOnlyList<ImportDocumentActivity> activities,
        CancellationToken cancellationToken = default);

    Task<Result<Error>> SetActivityResolutionAsync(
        ImportDocumentId id,
        ImportActivityId sourceId,
        ImportActivityResolution? resolution,
        CancellationToken cancellationToken = default);
    Task<Result<Error>> SetTagResolutionAsync(
        ImportDocumentId id,
        ImportTagId sourceId,
        ImportTagResolution? resolution,
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
