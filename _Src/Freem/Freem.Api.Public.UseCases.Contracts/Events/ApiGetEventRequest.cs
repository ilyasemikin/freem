using Freem.Domain.Events;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Events;

public sealed class ApiGetEventRequest : IRequest
{
    public RequestType Type => RequestTypes.Events.Get;

    public EventId Id { get; }

    public ApiGetEventRequest(EventId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
