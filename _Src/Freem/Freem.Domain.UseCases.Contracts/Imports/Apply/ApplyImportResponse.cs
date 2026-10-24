using Freem.Domain.Import;

namespace Freem.Domain.UseCases.Contracts.Imports.Apply;

public sealed class ApplyImportResponse
{
    public int RecordsCreated { get; }

    /// <summary>
    /// Rows from which a record cannot be created (for example, a period with zero duration).
    /// </summary>
    public IReadOnlyList<ImportRowId> SkippedRowIds { get; }

    public ApplyImportResponse(int recordsCreated, IReadOnlyList<ImportRowId> skippedRowIds)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(recordsCreated);
        ArgumentNullException.ThrowIfNull(skippedRowIds);

        RecordsCreated = recordsCreated;
        SkippedRowIds = skippedRowIds;
    }
}
