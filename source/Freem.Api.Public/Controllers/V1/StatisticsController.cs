using System.ComponentModel.DataAnnotations;
using Freem.Api.Public.Contracts.V1;
using Freem.Api.Public.Contracts.V1.Statistics;
using Freem.Api.Public.Extensions;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Statistics;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.V1;

/// <summary>Aggregated time statistics over the user's records.</summary>
[Route("api/v1/statistics")]
public sealed class StatisticsController : BaseController
{
    public StatisticsController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    /// <summary>Returns statistics summed over a single day, week, month or year.</summary>
    /// <param name="query">The day, week, month or year to aggregate.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <remarks>Day boundaries follow the user's day UTC offset setting, a week starts on the user's first day of the week.</remarks>
    [HttpGet("per-period")]
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
