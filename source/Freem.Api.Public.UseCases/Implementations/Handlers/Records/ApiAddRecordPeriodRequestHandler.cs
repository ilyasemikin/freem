using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.AddPeriod;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Records;
using Freem.Api.Public.UseCases.Implementations.Mappers;
using Freem.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.UseCases.Implementations.Handlers.Records;

internal sealed class ApiAddRecordPeriodRequestHandler : BaseAuthorizedRequestHandler<ApiAddRecordPeriodRequest>
{
    public ApiAddRecordPeriodRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiAddRecordPeriodRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<AddRecordPeriodRequest, Result<AddRecordPeriodResponse, Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
