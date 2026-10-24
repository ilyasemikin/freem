using Freem.Domain.Activities;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Activities;

public sealed class ApiArchiveActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.Archive;

    public ActivityId Id { get; }

    public ApiArchiveActivityRequest(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
