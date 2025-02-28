using System.ComponentModel.DataAnnotations;
using Freem.Entities.Statistics.Time;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Statistics.PerDays;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiStatisticsPerDaysRequest = Freem.Web.Api.Public.Contracts.Statistics.StatisticsPerDaysRequest;
using ApiStatisticsPerDaysResponse = Freem.Web.Api.Public.Contracts.Statistics.StatisticsPerDaysResponse;
using UseCaseStatisticsPerDaysRequest = Freem.Entities.UseCases.Contracts.Statistics.PerDays.StatisticsPerDaysRequest;
using UseCaseStatisticsPerDaysResponse = Freem.Entities.UseCases.Contracts.Statistics.PerDays.StatisticsPerDaysResponse;

namespace Freem.Web.Api.Public.Controllers.V1.Statistics;

[Authorize]
[Route("api/v1/statistics/per-days")]
public sealed class StatisticsPerDaysController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public StatisticsPerDaysController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiStatisticsPerDaysResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(
        [Required] [FromQuery] ApiStatisticsPerDaysRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(query);

        var response = await _executor.ExecuteAsync<UseCaseStatisticsPerDaysRequest, UseCaseStatisticsPerDaysResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Statistics)
            : CreateFailure(response.Error);
    }

    private static UseCaseStatisticsPerDaysRequest Map(ApiStatisticsPerDaysRequest request)
    {
        return new UseCaseStatisticsPerDaysRequest(request.Period);
    }

    private static IActionResult CreateSuccess(IReadOnlyDictionary<DateOnly, TimeStatistics> statistics)
    {
        var response = new ApiStatisticsPerDaysResponse(statistics);
        return new OkObjectResult(response);
    }

    private static IActionResult CreateFailure(Error<StatisticsPerDaysErrorCode> error)
    {
        return error.Code switch
        {
            StatisticsPerDaysErrorCode.UserNotFound => new UnauthorizedResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}