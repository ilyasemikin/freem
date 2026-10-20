using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Records.Remove;
using Freem.Web.Api.Public.UseCases.Context;
using Freem.Web.Api.Public.UseCases.Contracts.Records;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.UseCases.Implementations.Handlers.Records;

internal sealed class ApiRemoveRecordRequestHandler : BaseAuthorizedRequestHandler<ApiRemoveRecordRequest>
{
    public ApiRemoveRecordRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiRemoveRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<RemoveRecordRequest, Result<Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
