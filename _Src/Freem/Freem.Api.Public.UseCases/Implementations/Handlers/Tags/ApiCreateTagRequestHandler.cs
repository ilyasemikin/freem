using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Tags.Create;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Tags;
using Freem.Api.Public.UseCases.Implementations.Mappers;
using Freem.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.UseCases.Implementations.Handlers.Tags;

internal sealed class ApiCreateTagRequestHandler : BaseAuthorizedRequestHandler<ApiCreateTagRequest>
{
    public ApiCreateTagRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiCreateTagRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<CreateTagRequest, Result<CreateTagResponse, Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
