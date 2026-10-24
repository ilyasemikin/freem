using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.DataTransfer.Import;
using Freem.Api.Public.UseCases.Implementations.Mappers;
using Freem.Api.Public.UseCases.Implementations.Mappers.Requests.DataTransfer;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.DataTransfer;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.ListActivities;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.UseCases.Implementations.Handlers.DataTransfer.Import;

internal sealed class ApiListImportActivityRequestHandler : BaseAuthorizedRequestHandler<ApiListImportActivityRequest>
{
    public ApiListImportActivityRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiListImportActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<ListImportActivityRequest, Result<ListImportActivityResponse, Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
