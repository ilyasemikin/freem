using Freem.Api.Public.Contracts;
using Freem.Api.Public.UseCases.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.App;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.Infrastructure;

/// <summary>Service metadata. Anonymous.</summary>
[AllowAnonymous]
[Route("_meta")]
public sealed class MetaController : BaseController
{
    private readonly BuildInfo _buildInfo;

    public MetaController(IRequestMediator<ApiAuthorizedHandlerContext> mediator, BuildInfo buildInfo)
        : base(mediator)
    {
        ArgumentNullException.ThrowIfNull(buildInfo);

        _buildInfo = buildInfo;
    }

    /// <summary>Returns the running build version and the commit hash it was built from, if known.</summary>
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
