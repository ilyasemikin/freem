using Freem.Entities.Identifiers;

namespace Freem.Entities.UseCases.Contracts.Activities.Archive;

public sealed class ArchiveActivityRequest
{
    public ActivityIdentifier Id { get; }

    public ArchiveActivityRequest(ActivityIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}