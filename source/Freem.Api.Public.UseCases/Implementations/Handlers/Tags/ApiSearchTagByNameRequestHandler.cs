using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Tags.Search;
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

internal sealed class ApiSearchTagByNameRequestHandler : BaseAuthorizedRequestHandler<ApiSearchTagByNameRequest>
{
    public ApiSearchTagByNameRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiSearchTagByNameRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<SearchTagByNameRequest, Result<SearchTagByNameResponse, Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map(request.Name);
    }
}
