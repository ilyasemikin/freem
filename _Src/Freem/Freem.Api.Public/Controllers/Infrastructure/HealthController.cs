using Freem.Api.Public.UseCases.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.Infrastructure;

/// <summary>Service health probes for container orchestration. Anonymous.</summary>
[AllowAnonymous]
[Route("_health")]
public sealed class HealthController : BaseController
{
    public HealthController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    /// <summary>Reports that the process is alive and able to answer requests.</summary>
    [HttpGet("liveness")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetLiveness()
    {
        return Ok();
    }
}
