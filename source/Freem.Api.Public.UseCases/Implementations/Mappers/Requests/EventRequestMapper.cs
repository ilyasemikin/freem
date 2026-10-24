using Freem.Domain.UseCases.Contracts.Events.Get;
using Freem.Domain.UseCases.Contracts.Events.List;
using Freem.Domain.UseCases.Contracts.Filter;
using Freem.Api.Public.UseCases.Contracts.Events;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Requests;

internal static class EventRequestMapper
{
    public static GetEventRequest Map(this ApiGetEventRequest request)
    {
        return new GetEventRequest(request.Id);
    }

    public static ListEventRequest Map(this ApiListEventRequest request)
    {
        var limit = new Limit(request.Query.Limit);
        var offset = new Offset(request.Query.Offset);
        return new ListEventRequest(limit, offset)
        {
            EntityType = request.Query.EntityType,
            EntityRef = request.Query.EntityRef,
            From = request.Query.From,
            To = request.Query.To
        };
    }
}
