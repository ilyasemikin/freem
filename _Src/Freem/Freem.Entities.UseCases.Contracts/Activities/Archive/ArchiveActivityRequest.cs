using Freem.Domain.Activities;

namespace Freem.Entities.UseCases.Contracts.Activities.Archive;

public sealed class ArchiveActivityRequest
{
    public ActivityId Id { get; }

    public ArchiveActivityRequest(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}