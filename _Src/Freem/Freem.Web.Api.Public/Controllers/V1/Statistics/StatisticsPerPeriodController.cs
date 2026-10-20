using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Statistics.Time;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.Periods;
using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.Periods.Abstractions;
using Freem.Web.Api.Public.Contracts.DTO.Statistics.Periods;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;
using ApiStatisticsPerPeriodRequest = Freem.Web.Api.Public.Contracts.DTO.Statistics.StatisticsPerPeriodRequest;
using ApiStatisticsPerPeriodResponse = Freem.Web.Api.Public.Contracts.DTO.Statistics.StatisticsPerPeriodResponse;
using UseCaseStatisticsPerPeriodRequest = Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.StatisticsPerPeriodRequest;
using UseCaseStatisticsPerPeriodResponse = Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.StatisticsPerPeriodResponse;

namespace Freem.Web.Api.Public.Controllers.V1.Statistics;

[Route("api/v1/statistics/per-period")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiStatisticsPerPeriodResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class StatisticsPerPeriodController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;
    
    public StatisticsPerPeriodController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }
    
    [HttpGet]
    [EndpointSummary("Get summary activity statistics by period")]
    public async Task<IActionResult> GetAsync(
        [Required] [FromQuery] ApiStatisticsPerPeriodRequest query,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(query);
        
        var response = await _mediator.ExecuteAsync<UseCaseStatisticsPerPeriodRequest, Result<UseCaseStatisticsPerPeriodResponse, Error>>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Value.Statistics)
            : CreateFailure(response.Error);
    }

    private static UseCaseStatisticsPerPeriodRequest Map(ApiStatisticsPerPeriodRequest request)
    {
        IStatisticsPeriod period = request.Period switch
        {
            DayUnitPeriod day => new DayStatisticsPeriod(day.Day),
            MonthUnitPeriod month => new MonthStatisticsPeriod(month.Month),
            YearUnitPeriod year => new YearStatisticsPeriod(year.Year),
            _ => throw new InvalidOperationException()
        };
        
        return new UseCaseStatisticsPerPeriodRequest(period);
    }

    private static IActionResult CreateSuccess(TimeStatistics statistics)
    {
        var response = new ApiStatisticsPerPeriodResponse(statistics);
        return new OkObjectResult(response);
    }
    
    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.EntityNotFound)
            return new UnauthorizedResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}