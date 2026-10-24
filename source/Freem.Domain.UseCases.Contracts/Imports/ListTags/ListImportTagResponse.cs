using Freem.Domain.Entities.Import;
using Freem.Domain.UseCases.Contracts.Filter;

namespace Freem.Domain.UseCases.Contracts.Imports.ListTags;

public sealed class ListImportTagResponse
{
    public IReadOnlyList<ImportDocumentTag> Tags { get; }
    public TotalCount TotalCount { get; }

    public ListImportTagResponse(IReadOnlyList<ImportDocumentTag> tags, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(totalCount);

        Tags = tags;
        TotalCount = totalCount;
    }
}
