using Freem.Api.Public.Build;
using Freem.Api.Public.Constants;
using Freem.Api.Public.Contracts;
using Freem.Api.Public.UseCases.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.Infrastructure;

[AllowAnonymous]
[Route("_meta")]
[Tags(ControllerTags.Infrastructure)]
public sealed class MetaController : BaseController
{
    private readonly BuildInfo _buildInfo;

    public MetaController(IRequestMediator<ApiAuthorizedHandlerContext> mediator, BuildInfo buildInfo)
        : base(mediator)
    {
        ArgumentNullException.ThrowIfNull(buildInfo);

        _buildInfo = buildInfo;
    }

    [HttpGet("version")]
    [ProducesResponseType<VersionResponse>(StatusCodes.Status200OK)]
    public IActionResult GetVersion()
    {
        var response = new VersionResponse
        {
            Version = _buildInfo.Version,
            CommitHash = _buildInfo.CommitHash,
        };
        return Ok(response);
    }
}
