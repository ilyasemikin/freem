using System.Net.Mime;
using Freem.Api.Public.UseCases.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers;

/// <summary>Base for API controllers: requires authorization and exchanges JSON.</summary>
[ApiController]
[Authorize]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public abstract class BaseController : ControllerBase
{
    /// <summary>Dispatches API-level requests to their handlers.</summary>
    protected IRequestMediator<ApiAuthorizedHandlerContext> Mediator { get; }

    protected BaseController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        Mediator = mediator;
    }
}
