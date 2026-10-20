using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Records.PeriodList;
using Freem.Web.Api.Public.UseCases.Context;
using Freem.Web.Api.Public.UseCases.Contracts.Records;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Responses;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.UseCases.Implementations.Handlers.Records;

internal sealed class ApiListRecordByPeriodRequestHandler : BaseAuthorizedRequestHandler<ApiListRecordByPeriodRequest>
{
    public ApiListRecordByPeriodRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiListRecordByPeriodRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<PeriodListRequest, Result<PeriodListResponse, Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
