using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Events.List;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Events;
using Freem.Api.Public.UseCases.Implementations.Mappers;
using Freem.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.UseCases.Implementations.Handlers.Events;

internal sealed class ApiListEventRequestHandler : BaseAuthorizedRequestHandler<ApiListEventRequest>
{
    public ApiListEventRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiListEventRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<ListEventRequest, Result<ListEventResponse, Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
