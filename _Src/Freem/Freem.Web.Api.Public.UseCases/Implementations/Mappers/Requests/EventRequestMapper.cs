using Freem.Entities.UseCases.Contracts.Events.List;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.Web.Api.Public.UseCases.Contracts.Events;

namespace Freem.Web.Api.Public.UseCases.Implementations.Mappers.Requests;

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
