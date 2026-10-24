using Freem.Api.Public.Contracts.V1.Activities;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Activities;

public sealed class ApiListActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Activities.List;

    public ActivityListRequest Query { get; }

    public ApiListActivityRequest(ActivityListRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
