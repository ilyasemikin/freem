using Freem.Api.Public.Constants;
using Freem.Api.Public.UseCases.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.Infrastructure;

[AllowAnonymous]
[Route("_health")]
[Tags(ControllerTags.Infrastructure)]
public sealed class HealthController : BaseController
{
    public HealthController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    [HttpGet("liveness")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetLiveness()
    {
        return Ok();
    }
}
