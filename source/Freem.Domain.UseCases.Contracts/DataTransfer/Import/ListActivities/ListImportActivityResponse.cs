using Freem.Domain.Entities.DataTransfer.Import;
using Freem.Domain.UseCases.Contracts.Filter;

namespace Freem.Domain.UseCases.Contracts.DataTransfer.Import.ListActivities;

public sealed class ListImportActivityResponse
{
    public IReadOnlyList<ImportDocumentActivity> Activities { get; }
    public TotalCount TotalCount { get; }

    public ListImportActivityResponse(IReadOnlyList<ImportDocumentActivity> activities, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(totalCount);

        Activities = activities;
        TotalCount = totalCount;
    }
}
