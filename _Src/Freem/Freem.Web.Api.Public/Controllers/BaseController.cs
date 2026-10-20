using System.Net.Mime;
using Freem.Web.Api.Public.UseCases.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers;

[ApiController]
[Authorize]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public abstract class BaseController : ControllerBase
{
    protected IRequestMediator<ApiAuthorizedHandlerContext> Mediator { get; }

    protected BaseController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        Mediator = mediator;
    }
}
