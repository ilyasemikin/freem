using Freem.Domain.Activities;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Activities.Archive;

public sealed class ArchiveActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.Archive;

    public ActivityId Id { get; }

    public ArchiveActivityRequest(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}