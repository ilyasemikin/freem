using Freem.Domain.Import;

namespace Freem.Api.Public.Contracts.V1.Imports;

/// <summary>Result of applying an import.</summary>
public sealed class ImportApplyResponse
{
    /// <summary>Number of created records.</summary>
    public int RecordsCreated { get; }

    /// <summary>Rows from which a record cannot be created (for example, a period with zero duration); they are skipped.</summary>
    public IReadOnlyList<ImportRowId> SkippedRowIds { get; }

    public ImportApplyResponse(
        int recordsCreated,
        IReadOnlyList<ImportRowId> skippedRowIds)
    {
        ArgumentNullException.ThrowIfNull(skippedRowIds);

        RecordsCreated = recordsCreated;
        SkippedRowIds = skippedRowIds;
    }
}
