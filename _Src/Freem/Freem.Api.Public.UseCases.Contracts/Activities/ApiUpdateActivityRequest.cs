using Freem.Api.Public.Contracts.V1.Activities;
using Freem.Domain.Activities;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Activities;

public sealed class ApiUpdateActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.Update;

    public ActivityId Id { get; }
    public ActivityUpdateRequest Body { get; }

    public ApiUpdateActivityRequest(ActivityId id, ActivityUpdateRequest body)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(body);

        Id = id;
        Body = body;
    }
}
