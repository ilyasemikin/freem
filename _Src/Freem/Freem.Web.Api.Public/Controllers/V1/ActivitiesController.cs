using System.ComponentModel.DataAnnotations;
using Freem.Web.Api.Public.Contracts.Activities;
using Freem.Web.Api.Public.Controllers.V1.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1;

[Route("api/v1/activities")]
public sealed class ActivitiesController : BaseController
{
    [Authorize]
    [HttpPost]
    public Task<ActionResult<CreateActivityResponse>> CreateAsync(
        [Required] [FromBody] CreateActivityRequest request, 
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPut("{activityId}")]
    public Task<ActionResult> UpdateAsync(
        [Required] [FromRoute] string activityId,
        [Required] [FromBody] UpdateActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpDelete("{activityId}")]
    public Task<ActionResult> RemoveAsync(
        [Required] [FromRoute] string activityId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet("{activityId}/archive")]
    public Task<ActionResult> ArchiveAsync(
        [Required] [FromRoute] string activityId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost("{activityId}/unarchive")]
    public Task<ActionResult> UnarchiveAsync(
        [Required] [FromRoute] string activityId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet("{activityId}")]
    public Task<ActionResult<ActivityResponse>> GetAsync(
        [Required] [FromRoute] string activityId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet]
    public Task<ActionResult<IAsyncEnumerable<ActivityResponse>>> ListAsync(
        [Required] [FromQuery] ListActivityRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}