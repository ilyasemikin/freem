using Freem.Domain.Entities.DataTransfer.Import;
using Freem.Domain.UseCases.Contracts.Filter;

namespace Freem.Domain.UseCases.Contracts.DataTransfer.Import.List;

public sealed class ListImportResponse
{
    public IReadOnlyList<ImportDocument> Documents { get; }
    public TotalCount TotalCount { get; }

    public ListImportResponse(IReadOnlyList<ImportDocument> documents, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(documents);
        ArgumentNullException.ThrowIfNull(totalCount);

        Documents = documents;
        TotalCount = totalCount;
    }
}
