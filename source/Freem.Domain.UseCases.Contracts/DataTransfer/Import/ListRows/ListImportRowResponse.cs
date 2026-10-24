using Freem.Domain.Entities.DataTransfer.Import;
using Freem.Domain.UseCases.Contracts.Filter;

namespace Freem.Domain.UseCases.Contracts.DataTransfer.Import.ListRows;

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
