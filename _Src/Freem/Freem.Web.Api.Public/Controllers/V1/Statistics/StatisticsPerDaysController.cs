using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Statistics.Time;
using Freem.Entities.UseCases.Context;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;
using ApiStatisticsPerDaysRequest = Freem.Web.Api.Public.Contracts.DTO.Statistics.StatisticsPerDaysRequest;
using ApiStatisticsPerDaysResponse = Freem.Web.Api.Public.Contracts.DTO.Statistics.StatisticsPerDaysResponse;
using UseCaseStatisticsPerDaysRequest = Freem.Entities.UseCases.Contracts.Statistics.PerDays.StatisticsPerDaysRequest;
using UseCaseStatisticsPerDaysResponse = Freem.Entities.UseCases.Contracts.Statistics.PerDays.StatisticsPerDaysResponse;

namespace Freem.Web.Api.Public.Controllers.V1.Statistics;

[Route("api/v1/statistics/per-days")]
[Tags(ControllerTags.Statistics)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiStatisticsPerDaysResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class StatisticsPerDaysController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public StatisticsPerDaysController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpGet]
    [EndpointSummary("Get summary activity statistics by days")]
    public async Task<IActionResult> GetAsync(
        [Required] [FromQuery] ApiStatisticsPerDaysRequest query,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(query);

        var response = await _mediator.ExecuteAsync<UseCaseStatisticsPerDaysRequest, Result<UseCaseStatisticsPerDaysResponse, Error>>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Value.Statistics)
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

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.EntityNotFound)
            return new UnauthorizedResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}