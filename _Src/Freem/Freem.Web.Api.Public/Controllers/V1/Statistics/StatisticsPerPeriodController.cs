using System.ComponentModel.DataAnnotations;
using Freem.Entities.Statistics.Time;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod;
using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.Periods;
using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.Periods.Abstractions;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Autherization;
using Freem.Web.Api.Public.Contracts.DTO.Statistics.Periods;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiStatisticsPerPeriodRequest = Freem.Web.Api.Public.Contracts.DTO.Statistics.StatisticsPerPeriodRequest;
using ApiStatisticsPerPeriodResponse = Freem.Web.Api.Public.Contracts.DTO.Statistics.StatisticsPerPeriodResponse;
using UseCaseStatisticsPerPeriodRequest = Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.StatisticsPerPeriodRequest;
using UseCaseStatisticsPerPeriodResponse = Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.StatisticsPerPeriodResponse;

namespace Freem.Web.Api.Public.Controllers.V1.Statistics;

[Authorize(JwtAuthorizationPolicy.Name)]
[Route("api/v1/statistics/per-period")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiStatisticsPerPeriodResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class StatisticsPerPeriodController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;
    
    public StatisticsPerPeriodController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }
    
    [HttpGet]
    [EndpointSummary("Get summary activity statistics by period")]
    public async Task<IActionResult> GetAsync(
        [Required] [FromQuery] ApiStatisticsPerPeriodRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(query);
        
        var response = await _executor.ExecuteAsync<UseCaseStatisticsPerPeriodRequest, UseCaseStatisticsPerPeriodResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Statistics)
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
    
    private static IActionResult CreateFailure(Error<StatisticsPerPeriodErrorCode> error)
    {
        return error.Code switch
        {
            StatisticsPerPeriodErrorCode.UserNotFound => new UnauthorizedResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}