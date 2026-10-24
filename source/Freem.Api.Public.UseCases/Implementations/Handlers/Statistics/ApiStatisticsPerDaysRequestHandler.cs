using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Statistics.PerDays;
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

internal sealed class ApiStatisticsPerDaysRequestHandler : BaseAuthorizedRequestHandler<ApiStatisticsPerDaysRequest>
{
    public ApiStatisticsPerDaysRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiStatisticsPerDaysRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<StatisticsPerDaysRequest, Result<StatisticsPerDaysResponse, Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
