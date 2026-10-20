using System.ComponentModel.DataAnnotations;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.DTO;
using Freem.Web.Api.Public.Contracts.DTO.Statistics;
using Freem.Web.Api.Public.Extensions;
using Freem.Web.Api.Public.UseCases.Context;
using Freem.Web.Api.Public.UseCases.Contracts.Statistics;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers.V1;

[Route("api/v1/statistics")]
[Tags(ControllerTags.Statistics)]
public sealed class StatisticsController : BaseController
{
    public StatisticsController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    [HttpGet("per-days")]
    [EndpointSummary("Get summary activity statistics by days")]
    [ProducesResponseType<StatisticsPerDaysResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PerDaysAsync(
        [Required] [FromQuery] StatisticsPerDaysRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiStatisticsPerDaysRequest(query);
        return await Mediator.ExecuteAsync<ApiStatisticsPerDaysRequest, IActionResult>(context, request, cancellationToken);
    }

    [HttpGet("per-period")]
    [EndpointSummary("Get summary activity statistics by period")]
    [ProducesResponseType<StatisticsPerPeriodResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PerPeriodAsync(
        [Required] [FromQuery] StatisticsPerPeriodRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiStatisticsPerPeriodRequest(query);
        return await Mediator.ExecuteAsync<ApiStatisticsPerPeriodRequest, IActionResult>(context, request, cancellationToken);
    }
}
