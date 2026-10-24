using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Imports;
using Freem.Api.Public.UseCases.Implementations.Mappers;
using Freem.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Imports.Apply;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.UseCases.Implementations.Handlers.Imports;

internal sealed class ApiApplyImportRequestHandler : BaseAuthorizedRequestHandler<ApiApplyImportRequest>
{
    public ApiApplyImportRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiApplyImportRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainRequest = request.Map();
        if (!domainRequest.Success)
            return domainRequest.Error.MapError();

        var domainContext = context.Map();
        var response = await Mediator.ExecuteAsync<ApplyImportRequest, Result<ApplyImportResponse, Error>>(domainContext, domainRequest.Value, cancellationToken);
        return response.Map();
    }
}
