using Freem.Domain.Activities;

namespace Freem.Domain.Entities.DataTransfer.Import;

public sealed class ImportDocumentActivity
{
    public ImportActivity Activity { get; }

    /// <summary>
    /// Existing Freem activity the import activity is mapped to; null when not mapped yet.
    /// </summary>
    public ActivityId? MappedActivityId { get; }

    public ImportDocumentActivity(ImportActivity activity, ActivityId? mappedActivityId = null)
    {
        ArgumentNullException.ThrowIfNull(activity);

        Activity = activity;
        MappedActivityId = mappedActivityId;
    }
}
