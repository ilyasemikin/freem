using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Tags.GetByName;
using Freem.Web.Api.Public.UseCases.Context;
using Freem.Web.Api.Public.UseCases.Contracts.Tags;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Responses;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.UseCases.Implementations.Handlers.Tags;

internal sealed class ApiFindTagByNameRequestHandler : BaseAuthorizedRequestHandler<ApiFindTagByNameRequest>
{
    public ApiFindTagByNameRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiFindTagByNameRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<FindTagByNameRequest, Result<FindTagByNameResponse, Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map(request.Name);
    }
}
