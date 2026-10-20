using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Statistics.PerPeriod;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Statistics;
using Freem.Api.Public.UseCases.Implementations.Mappers;
using Freem.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.UseCases.Implementations.Handlers.Statistics;

internal sealed class ApiStatisticsPerPeriodRequestHandler : BaseAuthorizedRequestHandler<ApiStatisticsPerPeriodRequest>
{
    public ApiStatisticsPerPeriodRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiStatisticsPerPeriodRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<StatisticsPerPeriodRequest, Result<StatisticsPerPeriodResponse, Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
