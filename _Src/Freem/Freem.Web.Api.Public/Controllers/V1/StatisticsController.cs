using System.ComponentModel.DataAnnotations;
using Freem.Web.Api.Public.Contracts.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1;

[Route("api/statistics")]
public sealed class StatisticsController
{
    [HttpGet("per-days")]
    public Task<ActionResult<StatisticsPerDaysResponse>> GetPerDaysAsync(
        [Required] [FromQuery] StatisticsPerDaysRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}