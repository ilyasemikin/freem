using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Activities.List;
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

internal sealed class ApiListActivityRequestHandler : BaseAuthorizedRequestHandler<ApiListActivityRequest>
{
    public ApiListActivityRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiListActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<ListActivityRequest, Result<ListActivityResponse, Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
