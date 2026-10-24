using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Imports;
using Freem.Api.Public.UseCases.Implementations.Mappers;
using Freem.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Imports.ListRows;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.UseCases.Implementations.Handlers.Imports;

internal sealed class ApiListImportRowRequestHandler : BaseAuthorizedRequestHandler<ApiListImportRowRequest>
{
    public ApiListImportRowRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiListImportRowRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<ListImportRowRequest, Result<ListImportRowResponse, Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
