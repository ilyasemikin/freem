using Freem.Api.Public.Contracts.DTO.Events;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Events;

public sealed class ApiListEventRequest : IRequest
{
    public RequestType Type => RequestTypes.Events.List;

    public ListEventRequest Query { get; }

    public ApiListEventRequest(ListEventRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
