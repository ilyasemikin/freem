using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Activities.Archive;
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

internal sealed class ApiArchiveActivityRequestHandler : BaseAuthorizedRequestHandler<ApiArchiveActivityRequest>
{
    public ApiArchiveActivityRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiArchiveActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<ArchiveActivityRequest, Result<Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
