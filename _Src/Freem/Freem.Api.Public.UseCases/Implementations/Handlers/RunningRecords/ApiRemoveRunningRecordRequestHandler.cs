using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.RunningRecords.Remove;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.RunningRecords;
using Freem.Api.Public.UseCases.Implementations.Mappers;
using Freem.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.UseCases.Implementations.Handlers.RunningRecords;

internal sealed class ApiRemoveRunningRecordRequestHandler : BaseAuthorizedRequestHandler<ApiRemoveRunningRecordRequest>
{
    public ApiRemoveRunningRecordRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiRemoveRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<RemoveRunningRecordRequest, Result<Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
