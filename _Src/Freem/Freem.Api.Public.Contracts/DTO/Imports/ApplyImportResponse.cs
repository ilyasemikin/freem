using Freem.Domain.Import;

namespace Freem.Api.Public.Contracts.DTO.Imports;

/// <summary>Result of applying an import.</summary>
public sealed class ApplyImportResponse
{
    /// <summary>Number of created records.</summary>
    public int RecordsCreated { get; }

    /// <summary>Rows from which a record cannot be created (for example, a period with zero duration); they are skipped.</summary>
    public IReadOnlyList<ImportRowId> SkippedRowIds { get; }

    public ApplyImportResponse(
        int recordsCreated,
        IReadOnlyList<ImportRowId> skippedRowIds)
    {
        ArgumentNullException.ThrowIfNull(skippedRowIds);

        RecordsCreated = recordsCreated;
        SkippedRowIds = skippedRowIds;
    }
}
