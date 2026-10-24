using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.DataTransfer.Import;
using Freem.Api.Public.UseCases.Implementations.Mappers;
using Freem.Api.Public.UseCases.Implementations.Mappers.Requests.DataTransfer;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.DataTransfer;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.DataTransfer.Import.Get;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.UseCases.Implementations.Handlers.DataTransfer.Import;

internal sealed class ApiGetImportRequestHandler : BaseAuthorizedRequestHandler<ApiGetImportRequest>
{
    public ApiGetImportRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiGetImportRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<GetImportRequest, Result<GetImportResponse, Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
