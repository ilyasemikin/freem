using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Activities.Get;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Activities;
using Freem.Api.Public.UseCases.Implementations.Mappers;
using Freem.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.UseCases.Implementations.Handlers.Activities;

internal sealed class ApiGetActivityRequestHandler : BaseAuthorizedRequestHandler<ApiGetActivityRequest>
{
    public ApiGetActivityRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiGetActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<GetActivityRequest, Result<GetActivityResponse, Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
