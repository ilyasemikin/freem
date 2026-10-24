using Freem.Api.Public.Contracts.V1.Activities;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Activities;

public sealed class ApiCreateActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.Create;

    public ActivityCreateRequest Body { get; }

    public ApiCreateActivityRequest(ActivityCreateRequest body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }
}
