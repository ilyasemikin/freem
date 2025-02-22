using System.ComponentModel.DataAnnotations;
using Freem.Web.Api.Public.Contracts.Events;
using Freem.Web.Api.Public.Controllers.V1.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1;

[Route("api/v1/events")]
public sealed class EventsController : BaseController
{
    [Authorize]
    [HttpGet]
    public Task<ActionResult<IAsyncEnumerable<EventResponse>>> ListAsync(
        [Required] [FromQuery] ListEventRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}