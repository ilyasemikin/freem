using Freem.Entities.Activities.Identifiers;

namespace Freem.Entities.UseCases.DTO.Activities.Archive;

public sealed class ArchiveActivityRequest
{
    public ActivityIdentifier Id { get; }

    public ArchiveActivityRequest(ActivityIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}