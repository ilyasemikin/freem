using Freem.Domain.Entities.Import;
using Freem.Domain.UseCases.Contracts.Filter;

namespace Freem.Domain.UseCases.Contracts.Imports.ListRows;

public sealed class ListImportRowResponse
{
    public IReadOnlyList<ImportRow> Rows { get; }
    public TotalCount TotalCount { get; }

    public ListImportRowResponse(IReadOnlyList<ImportRow> rows, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentNullException.ThrowIfNull(totalCount);

        Rows = rows;
        TotalCount = totalCount;
    }
}
