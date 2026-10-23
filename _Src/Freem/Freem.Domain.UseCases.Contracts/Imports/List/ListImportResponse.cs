using Freem.Domain.Entities.Import;
using Freem.Domain.UseCases.Contracts.Filter;

namespace Freem.Domain.UseCases.Contracts.Imports.List;

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
