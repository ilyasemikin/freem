using Freem.Domain.Events;
using Freem.Domain.UseCases.Contracts.Filter;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Events.List;

public sealed class ListEventRequest : IRequest
{
    public RequestType Type => RequestTypes.Events.List;

    public Limit Limit { get; }
    public Offset Offset { get; }

    public EntityType? EntityType { get; init; }
    public string? EntityRef { get; init; }

    public DateTimeOffset? From { get; init; }
    public DateTimeOffset? To { get; init; }

    public ListEventRequest()
        : this(Limit.DefaultValue, Offset.DefaultValue)
    {
    }

    public ListEventRequest(Limit limit, Offset offset)
    {
        Limit = limit;
        Offset = offset;
    }
}
