using Freem.Domain.Activities;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Activities.Get;

public sealed class GetActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.Get;

    public ActivityId Id { get; }

    public GetActivityRequest(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}