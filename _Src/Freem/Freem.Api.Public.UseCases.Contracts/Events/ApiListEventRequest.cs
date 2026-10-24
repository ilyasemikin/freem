using Freem.Api.Public.Contracts.V1.Events;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Events;

public sealed class ApiListEventRequest : IRequest
{
    public RequestType Type => RequestTypes.Events.List;

    public EventListRequest Query { get; }

    public ApiListEventRequest(EventListRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
