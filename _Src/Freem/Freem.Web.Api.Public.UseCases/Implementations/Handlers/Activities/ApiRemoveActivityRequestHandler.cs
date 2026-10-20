using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Activities.Remove;
using Freem.Web.Api.Public.UseCases.Context;
using Freem.Web.Api.Public.UseCases.Contracts.Activities;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.UseCases.Implementations.Handlers.Activities;

internal sealed class ApiRemoveActivityRequestHandler : BaseAuthorizedRequestHandler<ApiRemoveActivityRequest>
{
    public ApiRemoveActivityRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiRemoveActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<RemoveActivityRequest, Result<Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
