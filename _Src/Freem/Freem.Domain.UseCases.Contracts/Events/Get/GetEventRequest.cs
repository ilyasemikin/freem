using Freem.Domain.Events;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Events.Get;

public sealed class GetEventRequest : IRequest
{
    public RequestType Type => RequestTypes.Events.Get;

    public EventId Id { get; }

    public GetEventRequest(EventId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
