using Freem.Domain.UseCases.Contracts.Events.List;
using Freem.Domain.UseCases.Contracts.Filter;
using Freem.Api.Public.UseCases.Contracts.Events;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Requests;

internal static class EventRequestMapper
{
    public static ListEventRequest Map(this ApiListEventRequest request)
    {
        var limit = new Limit(request.Query.Limit);
        return new ListEventRequest(limit)
        {
            After = request.Query.After
        };
    }
}
