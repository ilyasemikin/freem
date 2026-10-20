using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Statistics.PerDays;
using Freem.Web.Api.Public.UseCases.Context;
using Freem.Web.Api.Public.UseCases.Contracts.Statistics;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Responses;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.UseCases.Implementations.Handlers.Statistics;

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
