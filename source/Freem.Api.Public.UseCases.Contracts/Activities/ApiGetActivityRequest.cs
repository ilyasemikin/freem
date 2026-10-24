using Freem.Domain.Activities;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Activities;

public sealed class ApiGetActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.Get;

    public ActivityId Id { get; }

    public ApiGetActivityRequest(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
