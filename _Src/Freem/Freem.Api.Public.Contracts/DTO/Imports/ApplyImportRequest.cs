using Freem.Domain.Import;

namespace Freem.Api.Public.Contracts.DTO.Imports;

/// <summary>Request body for applying an import: either all rows or the listed ones.</summary>
public sealed class ApplyImportRequest
{
    /// <summary>True to apply all rows of the import; must not be combined with <see cref="RowIds"/>.</summary>
    public bool All { get; }

    /// <summary>Ids of the rows to apply.</summary>
    public IReadOnlyList<ImportRowId>? RowIds { get; }

    public ApplyImportRequest(bool all, IReadOnlyList<ImportRowId>? rowIds)
    {
        All = all;
        RowIds = rowIds;
    }
}
