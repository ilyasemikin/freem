using Freem.Entities.Activities.Identifiers;

namespace Freem.Entities.UseCases.Activities.Archive.Models;

public sealed class ArchiveActivityRequest
{
    public ActivityIdentifier Id { get; }

    public ArchiveActivityRequest(ActivityIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}